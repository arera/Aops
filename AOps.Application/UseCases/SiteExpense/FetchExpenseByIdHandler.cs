using AOps.Application.DTOs.Expense;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.SiteExpense
{
   public class FetchExpenseByIdHandler : IRequestHandler<FetchExpenseByIdCommand, EditExpenseDto>
    {
        private readonly ISiteExpenseRepository _repo;
        private readonly ILogger<FetchExpenseByIdHandler> _logger;
        private readonly string _siteUrl;

        public FetchExpenseByIdHandler(
            ISiteExpenseRepository repo,
            ILogger<FetchExpenseByIdHandler> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<EditExpenseDto> Handle(FetchExpenseByIdCommand request, CancellationToken cancellationToken)
        {
            var expense = await _repo.GetExpenseByIdAsync(request.ExpenseId, cancellationToken);

            return new EditExpenseDto
            {
                ExpenseId = expense.ExpenseId,
                ExpenseType = expense.ExpenseType,
                ExecutiveId = expense.ExecutiveId,
                ExpenseAmount = expense.ExpenseAmount,
                SiteId = expense.SiteId,
                ExpenseDate = expense.ExpenseDate,
                ExpenseRemark = expense.ExpenseRemark,
            };
        }
    }
}
