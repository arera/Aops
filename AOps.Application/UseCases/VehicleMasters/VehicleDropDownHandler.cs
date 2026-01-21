using AOps.Application.Interfaces;
using AOps.Application.UseCases.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VehicleMasters
{
    public class VehicleDropDownHandler : IRequestHandler<VehicleDropDownCommand,List<DTOs.Vehicle.VehicleDropdownDto>>
    {
        private IVehicleRepository _repo;

        public VehicleDropDownHandler(IVehicleRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<DTOs.Vehicle.VehicleDropdownDto>> Handle(VehicleDropDownCommand request, CancellationToken cancellationToken)
        {
            var vehicles = await _repo.GetVehicleDropdownAsync(cancellationToken);
            return vehicles.Select(vehicle => new DTOs.Vehicle.VehicleDropdownDto
            {
                VehicleId = vehicle.VehicleId,
                VehicleNumber = vehicle.VehicleNumber,
            }).ToList();
        }
    }
}
