using AOps.Application.Interfaces;
using AOps.Application.UseCases.VendorContracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VehicleMasters
{
    public class GetAllVehicleHandler : IRequestHandler<GetAllVehicleCommand, List<DTOs.Vehicle.FetchVehicleDto>>
    {
        private readonly IVehicleRepository _repo;
        private readonly ILogger<GetAllVehicleCommand> _logger;
        private readonly string _siteUrl;

        public GetAllVehicleHandler(
            IVehicleRepository repo,
            ILogger<GetAllVehicleCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<List<DTOs.Vehicle.FetchVehicleDto>> Handle(GetAllVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehiclemaster = await _repo.GetAllAsync(cancellationToken);

            return vehiclemaster.Select(vmaster => new DTOs.Vehicle.FetchVehicleDto
            {
                VendorId = vmaster.VendorId,
                VendorName = vmaster.Vendor?.VendorName ?? string.Empty,
                VehicleFuelType = vmaster.VehicleFuelType,
                VehicleId = vmaster.VehicleId,
                VehicleNumber = vmaster.VehicleNumber,
                VehicleModel = vmaster.VehicleModel,
                AmbulanceType = vmaster.AmbulanceType,
                VehicleType = vmaster.VehicleType,
                Registered_on = vmaster.CreatedAt,
                VendorContractId = vmaster.VendorContractId,
                RegistrationDocumentPath = !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(vmaster.RegistrationDocument)
                    ? $"{_siteUrl.TrimEnd('/')}/VehicleDocument/{vmaster.VehicleId}/{vmaster.RegistrationDocument}"
                    : null
            }).ToList();
        }
    }

}
