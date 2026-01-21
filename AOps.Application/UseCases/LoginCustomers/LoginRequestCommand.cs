using AOps.Application.DTOs;
using AOps.Application.DTOs.CustomerLogins;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LoginCustomers
{
    public record LoginRequestCommand(LoginRequestDto obj):IRequest<CustomerLoginResponseDto>;
    
}
