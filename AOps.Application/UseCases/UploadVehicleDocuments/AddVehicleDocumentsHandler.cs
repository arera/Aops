using AOps.Application.Interfaces;
using AOps.Application.UseCases.VendorContracts;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.UploadVehicleDocuments
{
    public class AddVehicleDocumentsHandler:IRequestHandler<AddVehicleDocumentsCommand,int>
    {
        public readonly IVehicleDocumentRepository _repo;
        public readonly ILogger<AddVehicleDocumentsCommand> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public AddVehicleDocumentsHandler(IVehicleDocumentRepository repository, ICommonRepository commonRepository, ILogger<AddVehicleDocumentsCommand> loggerRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }
        public async Task<int> Handle(AddVehicleDocumentsCommand request, CancellationToken cancellationToken)
        {
            string? documentPath = null;

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
                }

                // Step 2: Create contract entity
                var vehicledoc = new VehicleDocument
                {
                    VehicleId = request.obj.VehicleId,
                    DocumentType = request.obj.DocumentList.ToString(),
                    ExpiryDate = request.obj.ExpiryDate,
                    FilePath = documentPath ?? string.Empty,
                    VehicleDocumentId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    UploadedBy = Guid.NewGuid().ToString()
                };

                // Step 3: Save to DB
                var id = await _repo.AddAsync(vehicledoc, cancellationToken);
                return id;
            }
            catch (Exception ex)
            {
                // Step 4: Delete the file from local storage if it was saved
                if (!string.IsNullOrEmpty(documentPath))
                {
                    try
                    {
                        if (File.Exists(documentPath))
                        {
                            File.Delete(documentPath);
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        _loggerRepository.LogWarning(deleteEx, $"Failed to delete uploaded file at: {documentPath}");
                    }
                }

                // Log and rethrow the original exception
                _loggerRepository.LogError(ex, "Error inserting vendor contract");
                throw;
            }
        }
    }
}
