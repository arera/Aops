using AOps.Application.DTOs.CustomerSites;
using AOps.Application.DTOs.DropDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ILookupRepository
    {
        Task<List<SiteSelectDto>> GetSiteByCustomerAsync(Guid CustomerId, CancellationToken cancellationToken);
        Task<List<SiteSelectDto>> GetSiteByCustomerAsync(CancellationToken cancellationToken);
        Task<List<VehicleSelectDto>> GetVehcleBySiteAsync(Guid SiteId, CancellationToken cancellationToken);
        Task<List<UsersByRoleDto>> GetUsersByRoleAsync(int RoleId, CancellationToken cancellationToken);

        Task<List<EmployeeSelectDto>> GetAllEmployeeAsync(CancellationToken cancellationToken);

        Task<List<EmployeeSelectDto>> GetEmployeeBySiteAsync(Guid SiteId, CancellationToken cancellationToken);
    }
}
