using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.SiteExpense
{
    public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, int>
    {
        private readonly ISiteExpenseRepository _repo;
        private readonly ILogger<UpdateExpenseHandler> _logger;
        private readonly ICommonRepository _commonRepository;

        public UpdateExpenseHandler(
            ISiteExpenseRepository repo,
            ILogger<UpdateExpenseHandler> logger,
            ICommonRepository commonRepository)
        {
            _repo = repo;
            _logger = logger;
            _commonRepository = commonRepository;
        }

        public async Task<int> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            if (request?.obj == null)
            {
                _logger.LogWarning("UpdateExpenseCommand object is null.");
                throw new ArgumentNullException(nameof(request.obj), "Expense details cannot be null.");
            }

            // Step 1: Fetch existing record
            var existingExpense = await _repo.GetExpenseByIdAsync(request.obj.ExpenseId, cancellationToken);
            if (existingExpense == null)
            {
                throw new KeyNotFoundException($"Expense with ID {request.obj.ExpenseId} not found.");
            }

            string? receiptPath = null;

            try
            {
                // Step 2: Save (overwrite) receipt file if uploaded
                if (request.obj.ReceiptDocumentFile != null)
                {
                    // If the file name and path are same, the SaveFileAsync method should overwrite it.
                    // You can handle overwrite logic inside SaveFileAsync, or simply replace it here.
                    receiptPath = await _commonRepository.SaveFileAsync(
                        request.obj.ReceiptDocumentFile,
                        "SiteExpenseReceipt",
                        request.obj.ExpenseId.ToString(),
                        cancellationToken
                        
                    );
                }

                // Step 3: Update entity fields
                var site_expense = new SiteExpenses
                {
                    ExpenseId = request.obj.ExpenseId,
                    ExpenseType = request.obj.ExpenseType,
                    ExpenseAmount = request.obj.ExpenseAmount,
                    ExpenseDate = request.obj.ExpenseDate,
                    ExpenseRemarks = request.obj.ExpenseRemark,
                    ExecutiveId = request.obj.ExecutiveId,
                    SiteId = request.obj.SiteId,
                    ExpenseReceipt = receiptPath ?? string.Empty
                };
                

                // Step 4: Save changes to DB
                var result = await _repo.UpdateExpenseAsync(site_expense, cancellationToken);

                _logger.LogInformation("Successfully updated Site Expense with ID: {ExpenseId}", request.obj.ExpenseId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Site Expense record for ID: {ExpenseId}", request.obj.ExpenseId);
                throw;
            }
        }
    }
}
