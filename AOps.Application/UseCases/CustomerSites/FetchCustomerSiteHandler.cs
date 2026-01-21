using AOps.Application.DTOs.CustomerSites;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.CustomerSites
{
    public class FetchCustomerSiteHandler : IRequestHandler<FetchCustomerSiteCommand, List<GetCustomerSiteDto>>
    {
        private ICustomerSiteRepository _repo;
        private ILogger<FetchCustomerSiteHandler> _logger;
        private readonly string _siteUrl;

        public FetchCustomerSiteHandler(ICustomerSiteRepository repo, ILogger<FetchCustomerSiteHandler> logger, string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<List<GetCustomerSiteDto>> Handle(FetchCustomerSiteCommand request, CancellationToken cancellationToken)
        {
            var customersite = await _repo.GetAllAsync(cancellationToken);

            return customersite.Select(site => new GetCustomerSiteDto
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                SiteCity = site.SiteCity,
                SiteState = site.SiteState,
                SiteZip = site.SiteZip,
                SiteCountry = site.SiteCountry,
                CustomerName = site.CustomerName,
                CustomerId = site.CustomerId,
                CustomerContract = site.CustomerContract, 
                Vehicle_number_list = site.Vehicle_number_list,
                AssignedVehicleIds = site.AssignedVehicleIds,
                AssignedEmployeeIds = site.AssignedEmployeeIds,
                Employee_name_list = site.Employee_name_list
            }).ToList();

        }
    }
}
