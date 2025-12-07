using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.UploadVehicleDocuments
{
    public class FetchVehicleDocumentsHandler : IRequestHandler<FetchVehicleDocumentsCommand, List<DTOs.VehicleDocuments.FetchVehicleDocumentsDto>>
    {
        private readonly IVehicleDocumentRepository _repo;
        private readonly ILogger<FetchVehicleDocumentsHandler> _logger;
        private readonly string _siteUrl;

        public FetchVehicleDocumentsHandler(
            IVehicleDocumentRepository repo,
            ILogger<FetchVehicleDocumentsHandler> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<DTOs.VehicleDocuments.FetchVehicleDocumentsDto>> Handle(FetchVehicleDocumentsCommand request, CancellationToken cancellationToken)
        {
            var vehicledoc = await _repo.GetAllAsync(cancellationToken);

            return vehicledoc.Select(vmaster => new DTOs.VehicleDocuments.FetchVehicleDocumentsDto
            {
                VehicleNumber = vmaster.Vehicle.VehicleNumber,
                DocumentName = vmaster.DocumentType,
                ExpireOn = vmaster.ExpiryDate,
                UpdatedOn = vmaster.CreatedAt,
                DocumentId = vmaster.VehicleDocumentId,
                VehicleId = vmaster.VehicleId,
                DocumentPath = !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(vmaster.FilePath)
                    ? $"{_siteUrl.TrimEnd('/')}/VehicleDocument/{vmaster.VehicleId}/{vmaster.FilePath}"
                    : null

            }).ToList();
        }
    }
}
