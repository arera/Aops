using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VehicleMasters
{
    public record VehicleDropDownCommand : IRequest<List<DTOs.Vehicle.VehicleDropdownDto>>;

}
