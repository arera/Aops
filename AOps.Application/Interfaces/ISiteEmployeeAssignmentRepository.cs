using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ISiteEmployeeAssignmentRepository
    {
        Task<int> AddSiteEmployeeAsync(IEnumerable<SiteEmployeeAssignment> assignments, CancellationToken cancellationToken);
        Task<List<SiteEmployeeAssignment>> GetAssignmentsNotInAsync(Guid siteId, List<Guid> employeeIds, CancellationToken cancellationToken);
        Task RemoveEmployeeAsync(IEnumerable<SiteEmployeeAssignment> assignments, CancellationToken cancellationToken);
    }
}
