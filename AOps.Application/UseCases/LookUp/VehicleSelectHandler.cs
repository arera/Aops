using AOps.Application.DTOs.DropDown;
using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LookUp
{
     public class VehicleSelectHandler : IRequestHandler<VehicleSelectCommand, List<VehicleSelectDto>>
    {
        private ILookupRepository _repo;

        public VehicleSelectHandler(ILookupRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<VehicleSelectDto>> Handle(VehicleSelectCommand request, CancellationToken cancellationToken)
        {
            var vehicles = await _repo.GetVehcleBySiteAsync(request.SiteId, cancellationToken);
            if (vehicles == null || !vehicles.Any())
            {
                return new List<VehicleSelectDto>();
            }

            var vehicleDtos = vehicles.Select(v => new VehicleSelectDto
            {
                VehicleId = v.VehicleId,
                VehicleName = v.VehicleName
            }).ToList();

            return vehicleDtos;
        }

    }
}
