using AOps.Application.DTOs.AdminServiceTickets;
using AOps.Application.DTOs.CustomerTickets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminCustomerTicket
{
    public record FetchAllTicketsRaisedCommand() : IRequest<List<FetchAllTicketsDto>>;
}
