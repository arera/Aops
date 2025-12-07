using AOps.Application.Interfaces;
using AOps.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.UploadVehicleDocuments
{
    public class FetchVehiclDocumentByIdHandler: IRequestHandler<FetchVehiclDocumentByIdCommand, DTOs.VehicleDocuments.FetchVehicleDocumentsDto>
    {
        private readonly IVehicleDocumentRepository _repo;
        private readonly ILogger<FetchVehiclDocumentByIdHandler> _logger;
        private readonly string _siteUrl;

        public FetchVehiclDocumentByIdHandler(
           IVehicleDocumentRepository repo,
           ILogger<FetchVehiclDocumentByIdHandler> logger,
           string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<DTOs.VehicleDocuments.FetchVehicleDocumentsDto> Handle(FetchVehiclDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            var vehicledoc = await _repo.GetByIdAsync(request.DocumentId, cancellationToken);

            if (vehicledoc == null)
            {
                // You can throw, return a default object, or handle gracefully
                return new DTOs.VehicleDocuments.FetchVehicleDocumentsDto(); // Adjust based on your strategy
            }

            return new DTOs.VehicleDocuments.FetchVehicleDocumentsDto
            {
                VehicleId = vehicledoc.VehicleId,
                DocumentName = vehicledoc.DocumentType,
                ExpireOn = vehicledoc.ExpiryDate,
                UpdatedOn = vehicledoc.CreatedAt,
                DocumentId = vehicledoc.VehicleDocumentId,
                DocumentPath = !string.IsNullOrWhiteSpace(vehicledoc.FilePath)
                    ? $"{_siteUrl?.TrimEnd('/')}/VehicleDocument/{vehicledoc.VehicleId}/{vehicledoc.FilePath}"
                    : null
            };
        }
    }
}
