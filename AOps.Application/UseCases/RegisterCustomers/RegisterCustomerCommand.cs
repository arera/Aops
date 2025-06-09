using AOps.Application.DTOs;
using AOps.Domain.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterCustomers
{
    //public record RegisterCustomerCommand(string Name, string Email, Address Address, string Mobile, string GST) : IRequest<int>;
    public record RegisterCustomerCommand(
    string Name,
    string Email,
    AddressDto Address,
    string PrimaryMobile,
    string? SecondaryMobile,
    string GST
) : IRequest<int>;
}
