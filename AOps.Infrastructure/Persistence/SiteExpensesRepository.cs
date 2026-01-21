using AOps.Application.DTOs.CustomerSites;
using AOps.Application.DTOs.Expense;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class SiteExpensesRepository : ISiteExpenseRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<SiteExpensesRepository> _logger;
        public SiteExpensesRepository(AOpsDbContext context, ILogger<SiteExpensesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Guid> AddAsync(SiteExpenses siteexpense, CancellationToken cancellationToken = default)
        {
            await _context.SiteExpenses.AddAsync(siteexpense, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return siteexpense.ExpenseId;
        }

        public async Task<IEnumerable<FetchExpenseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var query =
                from e in _context.SiteExpenses
                join s in _context.CustomerSite on e.SiteId equals s.SiteId
                join o in _context.OrganisationLevels on e.ExecutiveId equals o.UserID
                select new FetchExpenseDto
                {
                    ExpenseId = e.ExpenseId,
                    SiteName = s.SiteName,              
                    ExecutiveName = o.Name,  
                    ExpenseName = e.ExpenseType.ToString(),
                    ExpenseAmount = e.ExpenseAmount,
                    ExpenseDate = e.ExpenseDate,
                    ExpenseRemark = e.ExpenseRemarks,
                    ExpenseReceipt = e.ExpenseReceipt,
                    Created_on = e.CreatedAt
                    
                };

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<EditExpenseDto> GetExpenseByIdAsync(Guid ExpenseId, CancellationToken cancellationToken = default)
        {
            var query =
                from e in _context.SiteExpenses
                join s in _context.CustomerSite on e.SiteId equals s.SiteId
                join o in _context.OrganisationLevels on e.ExecutiveId equals o.UserID
                where e.ExpenseId == ExpenseId
                select new EditExpenseDto
                {
                    ExpenseId = e.ExpenseId,
                    SiteId = s.SiteId,
                    ExecutiveId = o.UserID,
                    ExpenseType = e.ExpenseType,
                    ExpenseAmount = e.ExpenseAmount,
                    ExpenseDate = e.ExpenseDate,
                    ExpenseRemark = e.ExpenseRemarks,
                };

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> UpdateExpenseAsync(SiteExpenses expense, CancellationToken cancellationToken = default)
        {
            var exp = await _context.SiteExpenses.FirstOrDefaultAsync(x => x.ExpenseId == expense.ExpenseId, cancellationToken);
            if (exp == null)
            {
                throw new KeyNotFoundException("Expense not found.");
            }

            // Update fields
            exp.UpdatedAt = DateTime.UtcNow;
            exp.ExpenseDate = expense.ExpenseDate;
            exp.ExpenseAmount = expense.ExpenseAmount;
            exp.ExpenseRemarks = expense.ExpenseRemarks;
            exp.ExecutiveId = expense.ExecutiveId;
            exp.ExpenseType = expense.ExpenseType;
            if (!string.IsNullOrWhiteSpace(expense.ExpenseReceipt))
            {
                exp.ExpenseReceipt = expense.ExpenseReceipt;
            }
            return await _context.SaveChangesAsync(cancellationToken);

        }

    }
}
