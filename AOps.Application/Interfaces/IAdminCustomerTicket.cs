using AOps.Application.DTOs.AdminServiceTickets;
using AOps.Application.DTOs.CustomerSites;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IAdminCustomerTicket
    {
        Task<IEnumerable<FetchAllTicketsDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> UpdateTicketAsync(UpdateServiceTicketsDto ticketdto, CancellationToken cancellationToken = default);
        Task<FetchAllTicketsDto?> GetByIdAsync(Guid TicketId, CancellationToken cancellationToken = default);
    }
}
