using AOps.Application.DTOs.Vehicle;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VehicleMasters
{
    public record GetVehicleByIdCommand(Guid VehicleId) : IRequest<FetchVehicleDto>;

}
