using AOps.Application.DTOs.CustomerTickets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerTicket
{
    public record CreateTicketCommand(CreateTicketDto obj):IRequest<Guid>;
}
