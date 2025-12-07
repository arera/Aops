using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.UploadVehicleDocuments
{
    public class UpdateVehicleDocumentsHandler:IRequestHandler<UpdateVehicleDocumentsCommand,int>
    {

        public readonly IVehicleDocumentRepository _repo;
        public readonly ILogger<UpdateVehicleDocumentsHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public UpdateVehicleDocumentsHandler(IVehicleDocumentRepository repository, ILogger<UpdateVehicleDocumentsHandler> loggerRepository, ICommonRepository commonRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }

        public async Task<int> Handle(UpdateVehicleDocumentsCommand request, CancellationToken cancellationToken)
        {
            string? documentPath = null;
            string? oldFileToDelete = null;
            try
            {
                // Step 1: Save the file locally
                if (request.obj.DocumentFile != null)
                {
                    documentPath = await _commonRepository.SaveFileAsync(
                        request.obj.DocumentFile,
                        "VehicleDocument", request.obj.VehicleId.ToString(),
                        cancellationToken
                    );
                    if (!string.IsNullOrWhiteSpace(request.obj.OldFileUrl))
                    {
                        oldFileToDelete = request.obj.OldFileUrl;
                    }
                }

                // Step 2: Create contract entity
                var vdocument = new VehicleDocument
                {
                    VehicleId = request.obj.VehicleId,
                    DocumentType = request.obj.DocumentList.ToString(),
                    VehicleDocumentId = request.obj.DocumentId,
                    ExpiryDate = request.obj.ExpiryDate,
                    UpdatedAt = DateTime.UtcNow,
                    FilePath = documentPath ?? string.Empty,
                };
                // Step 3: Save to DB
                var id = await _repo.UpdateVehicleAsync(vdocument, cancellationToken);
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
                if (request.obj.DocumentFile != null && !string.IsNullOrEmpty(documentPath))
                {
                    try
                    {
                        var folderPath = Path.Combine("VehicleDocument", request.obj.VehicleId.ToString());
                        await _commonRepository.DeleteFileAsync(documentPath, folderPath);
                    }
                    catch (Exception deleteEx)
                    {
                        _loggerRepository.LogWarning(deleteEx, $"Failed to delete uploaded file at: {documentPath}");
                    }
                }

                _loggerRepository.LogError(ex, "Error updating vehicle record");
                throw;
            }
        }
    }
}
