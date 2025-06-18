using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IOrgLevelRepository
    {
        Task<int> AddAsync(Orglevels orglevel, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email,Guid UserId, CancellationToken cancellationToken = default);
        Task<Orglevels?> GetByUserIdAsync(Guid UserId, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(Orglevels orglevel, CancellationToken cancellationToken = default);
        Task<List<Orglevels>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> UpdatePasswordAsync(Guid UserID,string newPassword, CancellationToken cancellationToken = default);

    }
}
