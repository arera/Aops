using AOps.Application.DTOs.CustomerTickets;
using AOps.Application.DTOs.Expense;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ISiteExpenseRepository
    {
        Task<Guid> AddAsync(SiteExpenses expense, CancellationToken cancellationToken = default);
        Task<IEnumerable<FetchExpenseDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<EditExpenseDto> GetExpenseByIdAsync(Guid ExpenseId, CancellationToken cancellationToken = default);
        Task<int> UpdateExpenseAsync(SiteExpenses expc, CancellationToken cancellationToken = default);
    }
}
