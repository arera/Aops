using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using AOps.Domain.Entities;


namespace AOps.Application.UseCases.VehicleMasters
{
    public class AddVehicleHandler : IRequestHandler<AddVehicleCommand, int>
    {
        public readonly IVehicleRepository _repo;
        public readonly ILogger<AddVehicleCommand> _loggerRepository;
        public readonly ICommonRepository _commonRepository;


        public AddVehicleHandler(IVehicleRepository repository, ILogger<AddVehicleCommand> loggerRepository, ICommonRepository commonRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }

        public async Task<int> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleExists = await _repo.ExistsVehicleAsync(request.obj.VehicleNumber, cancellationToken);
            if (vehicleExists)
            {
                throw new Exception("Vehicle already exists."); // Or return a validation error accordingly
            }
            string? rcbookPath = null;
            Guid vehicleId = Guid.NewGuid();

            try
            {
                // Step 1: Save the file locally
                if (request.obj.RegistrationDocumentFile != null)
                {
                    rcbookPath = await _commonRepository.SaveFileAsync(
                        request.obj.RegistrationDocumentFile,
                        "VehicleDocument", vehicleId.ToString(),
                        cancellationToken
                    );
                }

                // Step 2: Create contract entity
                var vmaster = new VehicleMaster
                {
                    VehicleId = vehicleId,
                    VehicleNumber = request.obj.VehicleNumber,
                    VehicleFuelType = request.obj.VehicleFuelType,
                    VehicleModel = request.obj.VehicleModel,
                    VehicleType = request.obj.VehicleType,
                    VendorId = request.obj.VendorId,
                    AmbulanceType = request.obj.AmbulanceType,
                    CreatedAt = DateTime.UtcNow,
                    VendorContractId = string.Empty,
                    RegistrationDocument = rcbookPath??string.Empty
                };
                // Step 3: Save to DB
                 var id = await _repo.AddAsync(vmaster, cancellationToken);
                return id;
            }
            catch (Exception ex)
            {
                // Step 4: Delete the file from local storage if it was saved
                if (!string.IsNullOrEmpty(rcbookPath))
                {
                    try
                    {
                        if (File.Exists(rcbookPath))
                        {
                            File.Delete(rcbookPath);
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        _loggerRepository.LogWarning(deleteEx, $"Failed to delete uploaded file at: {rcbookPath}");
                    }
                }

                // Log and rethrow the original exception
                _loggerRepository.LogError(ex, "Error inserting vendor contract");
                throw;
            }
        }
    }
}

