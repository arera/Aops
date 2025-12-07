using AOps.Application.DTOs.CustomerTickets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerTicket
{
    public record GetAllCustomerTicketCommand(Guid CustomerId):IRequest<List<GetCustomerTicketDto>>;
    
}
