using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterCustomers
{
    public record FetchAllCustomerByIdCommand(Guid Userid): IRequest<DTOs.Customer.CreateCustomerDto>;
    
}
