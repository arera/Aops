using AOps.Application.DTOs.AdminDashboard;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminDashboards
{

     public class ExpiredVehicleDocumentsHandler : IRequestHandler<ExpiredVehicleDocumentsCommand, List<VehicleDocumentExpiryDto>>
    {
        private readonly IAdminDashboardRepository _repo;
        private readonly ILogger<ExpiredVehicleDocumentsCommand> _logger;
        private readonly string _siteUrl;

        public ExpiredVehicleDocumentsHandler(
            IAdminDashboardRepository repo,
            ILogger<ExpiredVehicleDocumentsCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<VehicleDocumentExpiryDto>> Handle(ExpiredVehicleDocumentsCommand request, CancellationToken cancellationToken)
        {
            // Fetch customer from repository
            var vehicleMasterList = await _repo.GetAllExpiredVehicleDocumentsAsync(cancellationToken);

            // Map customerDto
            // Local helper function to build full document URL safely
            string BuildDocumentUrl(Guid vehicleId, string? fileName)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return null;

                var baseUrl = _siteUrl?.TrimEnd('/') ?? string.Empty;
                return $"{baseUrl}/VehicleDocument/{vehicleId}/{fileName}";
            }


            var expiredvehicleDto = vehicleMasterList.Select(vmaster => new VehicleDocumentExpiryDto
            {
                VehicleName = vmaster.VehicleName,

                PollutionCertificateExpiry = vmaster.PollutionCertificateExpiry,
                PollutionCertificateFile = BuildDocumentUrl(vmaster.VehicleId, vmaster.PollutionCertificateFile),

                InsuranceExpiry = vmaster.InsuranceExpiry,
                InsuranceFile = BuildDocumentUrl(vmaster.VehicleId, vmaster.InsuranceFile),

                FitnessCertificateExpiry = vmaster.FitnessCertificateExpiry,
                FitnessCertificateFile = BuildDocumentUrl(vmaster.VehicleId, vmaster.FitnessCertificateFile)
            }).ToList();



            return expiredvehicleDto;
        }
    }
}

