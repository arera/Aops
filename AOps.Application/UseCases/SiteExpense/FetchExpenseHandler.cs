using AOps.Application.DTOs.Expense;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.SiteExpense
{
    public class FetchExpenseHandler:IRequestHandler<FetchExpenseCommand, List<FetchExpenseDto>>
    {
        private readonly ISiteExpenseRepository _repo;
        private readonly ILogger<FetchExpenseHandler> _logger;
        private readonly string _siteUrl;

        public FetchExpenseHandler(
            ISiteExpenseRepository repo,
            ILogger<FetchExpenseHandler> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<FetchExpenseDto>> Handle(FetchExpenseCommand request, CancellationToken cancellationToken)
        {
            var expensemaster = await _repo.GetAllAsync(cancellationToken);

            return expensemaster.Select(expense => new FetchExpenseDto
            {
                ExpenseName = expense.ExpenseName,
                SiteName = expense.SiteName,
                ExecutiveName = expense.ExecutiveName,
                ExpenseAmount = expense.ExpenseAmount,
                ExpenseDate = expense.ExpenseDate,
                ExpenseId = expense.ExpenseId,
                ExpenseRemark = expense.ExpenseRemark,
                Created_on = expense.Created_on,
                ExpenseReceipt = !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(expense.ExpenseReceipt)
                    ? $"{_siteUrl.TrimEnd('/')}/SiteExpenseReceipt/{expense.ExpenseId}/{expense.ExpenseReceipt}"
                    : null

            }).ToList();
        }
    }
}
