using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.CustomerSites
{
    public class AddCustomerSiteHandler : IRequestHandler<AddCustomerSiteCommand, int>
    {
        private readonly ICustomerSiteRepository _repo;
        private readonly ILogger<AddCustomerSiteHandler> _logger;
        private readonly ISiteVehicleAssignmentRepository _vrepo;
        private readonly ISiteEmployeeAssignmentRepository _erepo;

        public AddCustomerSiteHandler(ICustomerSiteRepository repository,ISiteVehicleAssignmentRepository site_repo, ISiteEmployeeAssignmentRepository emp_repo, ILogger<AddCustomerSiteHandler> logger)
        {
            _repo = repository;
            _logger = logger;
            _vrepo = site_repo;
            _erepo = emp_repo;
        }

        public async Task<int> Handle(AddCustomerSiteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1️⃣ Create site entity
                var site = new CustomerSite
                {
                    SiteId = Guid.NewGuid(),
                    ContractId = request.Obj.CustomerContractId,
                    SiteName = request.Obj.SiteName,
                    SiteCity = request.Obj.SiteCity,
                    SiteState = request.Obj.SiteState,
                    SiteZip = request.Obj.SiteZip,
                    SiteCountry = request.Obj.SiteCountry
                };

                // 2️⃣ Save site first
                var siteId = await _repo.AddAsync(site, cancellationToken);

                // 3️⃣ Prepare vehicle assignments
                if (request.Obj.AssignedVehicleIds != null && request.Obj.AssignedVehicleIds.Count > 0)
                {
                    var assignments = request.Obj.AssignedVehicleIds
                        .Select(vehicleId => new SiteVehicleAssignment
                        {
                            SiteId = site.SiteId, // Use the Guid we set above
                            VehicleId = vehicleId,
                            AssignedDate = DateTime.UtcNow,
                            IsAssigned = true
                            //AssignedBy = 
                        })
                        .ToList();

                    // 4️⃣ Save employee assignments
                    await _vrepo.AddSiteVehicleAsync(assignments, cancellationToken);
                }
                if (request.Obj.AssignedmployeeIds != null && request.Obj.AssignedmployeeIds.Count > 0)
                {
                    var assignments = request.Obj.AssignedmployeeIds
                        .Select(employeeId => new SiteEmployeeAssignment
                        {
                            SiteId = site.SiteId, // Use the Guid we set above
                            EmployeeId = employeeId,
                            CreatedAt = DateTime.UtcNow,
                            IsDeleted = true
                            //AssignedBy = 
                        })
                        .ToList();

                    // 4️⃣ Save employee assignments
                    await _erepo.AddSiteEmployeeAsync(assignments, cancellationToken);
                }

                return siteId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting customer site and vehicle assignments");
                throw;
            }
        }


    }

}
