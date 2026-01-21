using AOps.Application.DTOs.AOESite;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AOESites
{
    public class FetchAllAOESiteMappingHandler : IRequestHandler<FetchAllAOESiteMappingCommand, List<GetAoeSiteDto>>
    {
        private readonly IAOESiteMappingRepository _repo;
        private readonly ILogger<FetchAllAOESiteMappingCommand> _logger;
        private readonly string _siteUrl;

        public FetchAllAOESiteMappingHandler(
            IAOESiteMappingRepository repo,
            ILogger<FetchAllAOESiteMappingCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<GetAoeSiteDto>> Handle(FetchAllAOESiteMappingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repo.GetAllAsync(cancellationToken);

                _logger.LogInformation("Fetched {Count} AOE site mappings", result.Count());

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all AOE site mappings");
                throw;
            }
        }
    }
}
