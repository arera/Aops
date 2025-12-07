using AOps.Application.DTOs.CustomerSites;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerSites
{
    public class FetchCustomerSiteByIdHandler : IRequestHandler<FetchCustomerSiteByIdCommand, GetCustomerSiteDto>
    {
        private ICustomerSiteRepository _repo;
        private ILogger<FetchCustomerSiteByIdHandler> _logger;
        private readonly string _siteUrl;

        public FetchCustomerSiteByIdHandler(ICustomerSiteRepository repo, ILogger<FetchCustomerSiteByIdHandler> logger, string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<GetCustomerSiteDto> Handle(FetchCustomerSiteByIdCommand request, CancellationToken cancellationToken)
        {
            var site = await _repo.GetSiteByIdAsync(request.Siteid, cancellationToken);

            return new GetCustomerSiteDto
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                SiteCity = site.SiteCity,
                SiteState = site.SiteState,
                SiteZip = site.SiteZip,
                SiteCountry = site.SiteCountry,
                CustomerName = site.CustomerName,
                CustomerContract = site.CustomerContract,
                Vehicle_number_list = site.Vehicle_number_list
            };

        }


    }
}
