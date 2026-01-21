using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.CustomerSites
{
    public class UpdateCustomerSiteHandler : IRequestHandler<UpdateCustomerSiteCommand, int>
    {
        private readonly ICustomerSiteRepository _repo;
        private readonly ISiteVehicleAssignmentRepository _vrepo;
        private readonly ILogger<UpdateCustomerSiteHandler> _logger;
        private readonly ISiteEmployeeAssignmentRepository _erepo;

        public UpdateCustomerSiteHandler(
            ICustomerSiteRepository repository,
            ISiteVehicleAssignmentRepository siteRepo,
            ISiteEmployeeAssignmentRepository empRepo,
            ILogger<UpdateCustomerSiteHandler> logger)
        {
            _repo = repository;
            _vrepo = siteRepo;
            _logger = logger;
            _erepo = empRepo;
        }

        public async Task<int> Handle(UpdateCustomerSiteCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Get the site from DB including assigned vehicles
            var site = await _repo.GetByIdWithVehiclesAsync(request.Obj.SiteId, cancellationToken);

            if (site == null)
                throw new KeyNotFoundException("Customer site not found");

            // 2️⃣ Update site details
            site.SiteName = request.Obj.SiteName;
            site.SiteCity = request.Obj.SiteCity;
            site.SiteState = request.Obj.SiteState;
            site.SiteZip = request.Obj.SiteZip;
            site.SiteCountry = request.Obj.SiteCountry;
            site.ContractId = request.Obj.CustomerContractId; // ✅ assuming FK property is ContractId

            // 3️⃣ Sync vehicles
            var existingVehicleIds = site.SiteVehicleAssignments
                .Select(sva => sva.VehicleId)
                .ToList();

            var newVehicleIds = request.Obj.AssignedVehicleIds ?? new List<Guid>();

            // 3️⃣ Sync employees
            var existingEmployeeIds = site.SiteEmployeeAssignments
                .Select(sva => sva.EmployeeId)
                .ToList();

            var newEmployeeIds = request.Obj.AssignedEmployeeIds ?? new List<Guid>();

            // Employees to add
            var employeesToAdd = newEmployeeIds.Except(existingEmployeeIds).ToList();
            foreach (var emp in employeesToAdd)
            {
                site.SiteEmployeeAssignments.Add(new SiteEmployeeAssignment
                {
                    SiteId = site.SiteId,
                    EmployeeId = emp
                });

            }

            // Vehicles to add
            var vehiclesToAdd = newVehicleIds.Except(existingVehicleIds).ToList();
            foreach (var vehicleId in vehiclesToAdd)
            {
                site.SiteVehicleAssignments.Add(new SiteVehicleAssignment
                {
                    SiteId = site.SiteId,
                    VehicleId = vehicleId
                });
            }

            // Vehicles to remove
            var vehiclesToRemove = site.SiteVehicleAssignments
                .Where(sva => !newVehicleIds.Contains(sva.VehicleId))
                .ToList();

            foreach (var remove in vehiclesToRemove)
            {
                site.SiteVehicleAssignments.Remove(remove);
            }

            // Employee to remove
            var employeesToRemove = site.SiteEmployeeAssignments
                .Where(sva => !newEmployeeIds.Contains(sva.EmployeeId))
                .ToList();

            foreach (var remove in employeesToRemove)
            {
                site.SiteEmployeeAssignments.Remove(remove);
            }

            // 4️⃣ Save changes via repository
            await _repo.UpdateSiteAsync(site, cancellationToken);

            _logger.LogInformation("Updated site {SiteId} with {VehicleCount} vehicles", site.SiteId, newVehicleIds.Count);

            return newVehicleIds.Count; // or return affected rows from repo
        }

    }
}
