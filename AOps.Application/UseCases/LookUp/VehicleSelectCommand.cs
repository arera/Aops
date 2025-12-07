using AOps.Application.DTOs.DropDown;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LookUp
{
    public record VehicleSelectCommand(Guid SiteId):IRequest<List<VehicleSelectDto>>;
   
}
