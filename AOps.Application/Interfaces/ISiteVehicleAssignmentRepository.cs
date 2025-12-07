using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ISiteVehicleAssignmentRepository
    {
        Task<int> AddSiteVehicleAsync(IEnumerable<SiteVehicleAssignment> assignments, CancellationToken cancellationToken);
        Task<List<SiteVehicleAssignment>> GetAssignmentsNotInAsync(Guid siteId, List<Guid> vehicleIds, CancellationToken cancellationToken);
        Task RemoveRangeAsync(IEnumerable<SiteVehicleAssignment> assignments, CancellationToken cancellationToken);
    }
}
