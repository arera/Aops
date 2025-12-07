using AOps.Application.Interfaces;
using AOps.Application.UseCases.VendorContracts;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AOESites
{
    public class AddAOEMappingHandler : IRequestHandler<AddAOEMappingCommand, Guid>
    {
        private readonly IAOESiteMappingRepository _repo;
        private readonly ILogger<AddAOEMappingCommand> _logger;

        public AddAOEMappingHandler(IAOESiteMappingRepository repository, ILogger<AddAOEMappingCommand> logger)
        {
            _repo = repository;
            _logger = logger;
        }

        public async Task<Guid> Handle(AddAOEMappingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null || request.obj == null)
                    throw new ArgumentNullException(nameof(request.obj));

                var aoeId = request.obj.AoeId;
                var siteIds = request.obj.SiteId;

                if (siteIds == null || !siteIds.Any())
                    throw new ArgumentException("At least one site must be selected");

                foreach (var siteId in siteIds)
                {
                    var mapping = new AOESiteMapping
                    {
                        MappingId = Guid.NewGuid(),
                        OrgEmployeeId = aoeId,
                        SiteId = siteId,
                        CreatedAt = DateTime.UtcNow,
                        //CreatedBy = "system"
                    };

                    await _repo.AddAsync(mapping, cancellationToken);
                }

                // await _repo.AddRangeAsync(mappings, cancellationToken);

                _logger.LogInformation("Successfully created {Count} mappings for AOE {AoeId}", aoeId);

                return aoeId; // or return Guid.NewGuid() if you want a batch id
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating AOE site mappings");
                throw;
            }
        }
    }

}

