using AOps.Application.DTOs.CustomerTickets;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICustomerTicketRepository
    {
        Task<Guid> AddAsync(CustomerTickets Tickets, CancellationToken cancellationToken = default);
        Task<IEnumerable<GetCustomerTicketDto>> GetAllAsync(Guid customerid, CancellationToken cancellationToken = default);

        Task<Guid> GetSitehandelByAsync(Guid SiteId, CancellationToken cancellationToken = default);

        Task<int> UpdateVehicleAsync(CustomerTickets Tickets, CancellationToken cancellationToken = default);
    }
}
