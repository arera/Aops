using AOps.Application.DTOs.DropDown;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.Vendor;
using AOps.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LookUp
{
    
   public class SiteSelectHandler : IRequestHandler<SiteSelectCommand, List<SiteSelectDto>>
    {
        private ILookupRepository _repo;

        public SiteSelectHandler(ILookupRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<SiteSelectDto>> Handle(SiteSelectCommand request, CancellationToken cancellationToken)
        {
            List<SiteSelectDto> sites;

            if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
            {
                sites = await _repo.GetSiteByCustomerAsync(request.CustomerId.Value, cancellationToken);
            }
            else
            {
                sites = await _repo.GetSiteByCustomerAsync(cancellationToken);
            }

            if (sites == null || !sites.Any())
            {
                return new List<SiteSelectDto>();
            }

            return sites.Select(site => new SiteSelectDto
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
            }).ToList();
        }

    }
}
