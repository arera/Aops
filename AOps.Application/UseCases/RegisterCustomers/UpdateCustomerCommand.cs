using AOps.Application.DTOs.Customer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterCustomers
{
   
  public record UpdateCustomerCommand(Guid CustomerId,string Name,
    string Email,
    string PrimaryMobile,
    string? SecondaryMobile,
    string GST,
    bool IsActive,
     AddressDto Address
) : IRequest<int>;
   
}
