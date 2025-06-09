using AOps.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterOrglevels
{
   public record RegisterOrgLevelsCommand(string Name,string Email,int Role,string? Mobile,string? passwordhash) : IRequest<int>;
}

