using AOps.Application.DTOs.CustomerDashboard;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerDashboards
{
    public class DashboardVehicleHandler: IRequestHandler<DashboardvehicleCommand, List<VehiclesDto>>
    {
        private readonly ICustomerDashboardsRepository _repo;
        private readonly ILogger<DashboardvehicleCommand> _logger;
        private readonly string _siteUrl;

        public DashboardVehicleHandler(
            ICustomerDashboardsRepository repo,
            ILogger<DashboardvehicleCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<List<VehiclesDto>> Handle(DashboardvehicleCommand request, CancellationToken cancellationToken)
        {
            // Fetch vehicles from repository
            var vehicleMasterList = await _repo.GetAllVehicleAsync(request.CustomerId, cancellationToken);

            // Map to VehiclesDto
            var vehiclesDto = vehicleMasterList.Select(vmaster => new VehiclesDto
            {
                VehicleName = vmaster.VehicleName,
                Insurance = vmaster.Insurance,
                PollutionCertificate = vmaster.PollutionCertificate,
                FitnessCertificate = vmaster.FitnessCertificate,
                Permit = vmaster.Permit,
                SiteName = vmaster.SiteName,
            }).ToList();

            return vehiclesDto;
        }

    }
}
