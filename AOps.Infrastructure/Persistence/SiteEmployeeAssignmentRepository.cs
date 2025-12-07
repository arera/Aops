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
    public class SiteEmployeeAssignmentRepository : ISiteEmployeeAssignmentRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<SiteEmployeeAssignmentRepository> _logger;

        public SiteEmployeeAssignmentRepository(AOpsDbContext context, ILogger<SiteEmployeeAssignmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddSiteEmployeeAsync(IEnumerable<SiteEmployeeAssignment> assignments, CancellationToken cancellationToken)
        {
            try
            {
                await _context.SiteEmployeeAssignment.AddRangeAsync(assignments, cancellationToken);
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding site employee assignments");
                throw;
            }
        }
        public async Task<List<SiteEmployeeAssignment>> GetAssignmentsNotInAsync(Guid siteId, List<Guid> employeeIds, CancellationToken cancellationToken)
        {
            return await _context.SiteEmployeeAssignment
                .Where(a => a.SiteId == siteId && !employeeIds.Contains(a.EmployeeId))
                .ToListAsync(cancellationToken);
        }

        public async Task RemoveEmployeeAsync(IEnumerable<SiteEmployeeAssignment> assignments, CancellationToken cancellationToken)
        {
            _context.SiteEmployeeAssignment.RemoveRange(assignments);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
