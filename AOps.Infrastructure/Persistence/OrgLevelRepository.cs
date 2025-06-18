using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class OrgLevelRepository : IOrgLevelRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<OrgLevelRepository> _logger;

        public OrgLevelRepository(AOpsDbContext context, ILogger<OrgLevelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(Orglevels orglevel, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.OrganisationLevels.AddAsync(orglevel, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return orglevel.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating customer with Email: {Email}", orglevel.Email);

                // Optional: You could check for specific constraint violations here
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating customer with Email: {Email}", orglevel.Email);
                throw;
            }
        }
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.OrganisationLevels.AnyAsync(c => c.Email == email, cancellationToken);
        }
        public async Task<bool> ExistsByEmailAsync(string email,Guid UserId, CancellationToken cancellationToken = default)
        {
            return await _context.OrganisationLevels.AnyAsync(c => c.Email == email && c.UserID != UserId, cancellationToken);
        }

        public async Task<Orglevels?> GetByUserIdAsync(Guid UserId, CancellationToken cancellationToken = default)
        {
            return await _context.OrganisationLevels.FirstOrDefaultAsync(u => u.UserID == UserId);
        }

        public async Task<int> UpdateAsync(Orglevels orglevel, CancellationToken cancellationToken = default)
        {
            var existing = await _context.OrganisationLevels
                .FirstOrDefaultAsync(x => x.UserID == orglevel.UserID, cancellationToken);

            if (existing == null)
            {
                throw new KeyNotFoundException($"User with ID {orglevel.UserID} not found.");
            }

            existing.Name = orglevel.Name;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.Email = orglevel.Email;
            existing.Mobile = orglevel.Mobile;
            existing.CreatedBy = orglevel.CreatedBy;
            existing.Role = orglevel.Role;
            existing.IsDeleted = orglevel.IsDeleted;
            // Update other properties as needed

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Orglevels>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.OrganisationLevels.OrderByDescending(o=> o.CreatedAt).ToListAsync();
        }

        public async Task<int> UpdatePasswordAsync(Guid userId, string newPasswordHash, CancellationToken cancellationToken = default)
        {
            var user = await _context.OrganisationLevels
                .FirstOrDefaultAsync(u => u.UserID == userId, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            user.Password_hash = newPasswordHash;
            user.UpdatedAt = DateTime.UtcNow; // optional audit
            //user.UpdatedBy = userId; // assuming the user is updating their own password

            return await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
