using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VehicleMasters
{
    public class UpdateVehicleHandler : IRequestHandler<UpdateVehicleCommand, int>
    {
        public readonly IVehicleRepository _repo;
        public readonly ILogger<UpdateVehicleCommand> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public UpdateVehicleHandler(IVehicleRepository repository, ILogger<UpdateVehicleCommand> loggerRepository, ICommonRepository commonRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }

        public async Task<int> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleExists = await _repo.ExistsVehicleAsync(request.obj.VehicleNumber,request.obj.VehicleId, cancellationToken);
            if (vehicleExists)
            {
                throw new Exception("Vehicle already exists."); // Or return a validation error accordingly
            }
            string? rcbookPath = null;
            string? oldFileToDelete = null;
            try
            {
                // Step 1: Save the file locally
                if (request.obj.RegistrationDocumentFile != null)
                {
                    rcbookPath = await _commonRepository.SaveFileAsync(
                        request.obj.RegistrationDocumentFile,
                        "VehicleDocument", request.obj.VehicleId.ToString(),
                        cancellationToken
                    );
                   
                }

                // Step 2: Create contract entity
                var vmaster = new VehicleMaster
                {
                    VehicleId = request.obj.VehicleId,
                    VehicleNumber = request.obj.VehicleNumber,
                    VehicleFuelType = request.obj.VehicleFuelType,
                    VehicleModel = request.obj.VehicleModel,
                    VehicleType = request.obj.VehicleType,
                    VendorId = request.obj.VendorId,
                    AmbulanceType = request.obj.AmbulanceType,
                    UpdatedAt = DateTime.UtcNow,
                    RegistrationDocument = rcbookPath ?? string.Empty,
                    VendorContractId = request.obj.VContractId
                };
                // Step 3: Save to DB
                var id = await _repo.UpdateVehicleAsync(vmaster, cancellationToken);
                if (!string.IsNullOrWhiteSpace(oldFileToDelete))
                {
                    var folderPath = Path.Combine("VehicleDocument", request.obj.VehicleId.ToString());
                    await _commonRepository.DeleteFileAsync(oldFileToDelete, folderPath);
                }
                return id;
            }
            catch (Exception ex)
            {
                // Only delete the new file if one was uploaded and saved
                if (request.obj.RegistrationDocumentFile != null && !string.IsNullOrEmpty(rcbookPath))
                {
                    try
                    {
                        var folderPath = Path.Combine("VehicleDocument", request.obj.VehicleId.ToString());
                        await _commonRepository.DeleteFileAsync(rcbookPath, folderPath); 
                    }
                    catch (Exception deleteEx)
                    {
                        _loggerRepository.LogWarning(deleteEx, $"Failed to delete uploaded file at: {rcbookPath}");
                    }
                }

                _loggerRepository.LogError(ex, "Error updating vehicle record");
                throw;
            }
            }

        }
    }

