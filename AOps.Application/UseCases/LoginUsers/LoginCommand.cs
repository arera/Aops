using AOps.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LoginUsers
{
    public record LoginCommand(string username,string password): IRequest<LoginResponseDto>;
   
}
