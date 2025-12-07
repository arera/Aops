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
    public class SiteVehicleAssignmentRepository : ISiteVehicleAssignmentRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<SiteVehicleAssignmentRepository> _logger;

        public SiteVehicleAssignmentRepository(AOpsDbContext context, ILogger<SiteVehicleAssignmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<int> AddSiteVehicleAsync(IEnumerable<SiteVehicleAssignment> assignments, CancellationToken cancellationToken)
        {
            try
            {
                await _context.SiteVehicleAssignment.AddRangeAsync(assignments, cancellationToken);
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding site vehicle assignments");
                throw;
            }
        }
        public async Task<List<SiteVehicleAssignment>> GetAssignmentsNotInAsync(Guid siteId, List<Guid> vehicleIds, CancellationToken cancellationToken)
        {
            return await _context.SiteVehicleAssignment
                .Where(a => a.SiteId == siteId && !vehicleIds.Contains(a.VehicleId))
                .ToListAsync(cancellationToken);
        }

        public async Task RemoveRangeAsync(IEnumerable<SiteVehicleAssignment> assignments, CancellationToken cancellationToken)
        {
            _context.SiteVehicleAssignment.RemoveRange(assignments);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
