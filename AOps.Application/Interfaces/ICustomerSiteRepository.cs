using AOps.Application.DTOs.CustomerSites;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICustomerSiteRepository
    {
        Task<int> AddAsync(CustomerSite contractsite, CancellationToken cancellationToken = default);
        Task<IEnumerable<GetCustomerSiteDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> UpdateSiteAsync(CustomerSite ccontract, CancellationToken cancellationToken = default);

        Task<GetCustomerSiteDto> GetSiteByIdAsync(Guid siteId, CancellationToken cancellationToken);

        Task<CustomerSite> GetByIdWithVehiclesAsync(Guid siteId, CancellationToken cancellationToken = default);

    }
}
