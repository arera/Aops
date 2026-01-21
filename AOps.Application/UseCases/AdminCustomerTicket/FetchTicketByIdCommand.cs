using AOps.Application.DTOs.AdminServiceTickets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminCustomerTicket
{
   public record FetchTicketByIdCommand(Guid Ticketid) : IRequest<FetchAllTicketsDto>;
}
