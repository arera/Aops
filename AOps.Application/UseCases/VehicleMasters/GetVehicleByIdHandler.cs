using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;

namespace AOps.Application.UseCases.VehicleMasters
{
    public class GetVehicleByIdHandler : IRequestHandler<GetVehicleByIdCommand, DTOs.Vehicle.FetchVehicleDto>
    {
        private readonly IVehicleRepository _repo;
        private readonly ILogger<GetVehicleByIdCommand> _logger;
        private readonly string _siteUrl;

        public GetVehicleByIdHandler(
            IVehicleRepository repo,
            ILogger<GetVehicleByIdCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<DTOs.Vehicle.FetchVehicleDto> Handle(GetVehicleByIdCommand request, CancellationToken cancellationToken)
        {
            var vmaster = await _repo.GetByIdAsync(request.VehicleId, cancellationToken);

            if (vmaster == null)
            {
                _logger.LogWarning("Vehicle not found with ID: {VehicleId}", request.VehicleId);
                return null; // Or throw a NotFoundException if that's your pattern
            }

            return new DTOs.Vehicle.FetchVehicleDto
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
                RegistrationDocumentPath = !string.IsNullOrWhiteSpace(vmaster.RegistrationDocument)
                    ? $"{_siteUrl.TrimEnd('/')}/VehicleDocument/{vmaster.VehicleId}/{vmaster.RegistrationDocument}": null
            };
        }
    }


}
