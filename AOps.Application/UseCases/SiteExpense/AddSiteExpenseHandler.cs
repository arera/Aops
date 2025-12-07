using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.SiteExpense
{
    public class AddSiteExpenseHandler: IRequestHandler<AddSiteExpenseCommand,Guid>
    {
        public readonly ISiteExpenseRepository _repo;
        public readonly ILogger<AddSiteExpenseHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;
        public AddSiteExpenseHandler(ISiteExpenseRepository repository, ILogger<AddSiteExpenseHandler> loggerRepository, ICommonRepository commonRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }
        public async Task<Guid> Handle(AddSiteExpenseCommand request, CancellationToken cancellationToken)
        {
            string? receiptpath = null;
            Guid ExpenseId = Guid.NewGuid();

            try
            {
                // Step 1: Save the file locally
                if (request.obj.ReceiptDocumentFile != null)
                {
                    receiptpath = await _commonRepository.SaveFileAsync(
                        request.obj.ReceiptDocumentFile,
                        "SiteExpenseReceipt", ExpenseId.ToString(),
                        cancellationToken
                    );
                }

                // Step 2: Create contract entity
                var sexpense = new SiteExpenses
                {
                    ExpenseId = ExpenseId,
                    ExpenseAmount = request.obj.ExpenseAmount,
                    ExpenseDate = request.obj.ExpenseDate,
                    ExpenseType = request.obj.ExpenseType,
                    ExecutiveId = request.obj.ExecutiveId,
                    ExpenseRemarks = request.obj.ExpenseRemark,
                    SiteId = request.obj.SiteId,
                    ExpenseReceipt = receiptpath ?? string.Empty
                };
                // Step 3: Save to DB
                var id = await _repo.AddAsync(sexpense, cancellationToken);
                return id;
            }
            catch (Exception ex)
            {
                // Step 4: Delete the file from local storage if it was saved
                if (!string.IsNullOrEmpty(receiptpath))
                {
                    try
                    {
                        if (File.Exists(receiptpath))
                        {
                            File.Delete(receiptpath);
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        _loggerRepository.LogWarning(deleteEx, $"Failed to delete uploaded file at: {receiptpath}");
                    }
                }

                // Log and rethrow the original exception
                _loggerRepository.LogError(ex, "Error inserting Site Expense");
                throw;
            }
        }
    }
}
