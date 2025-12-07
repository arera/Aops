using AOps.Application.DTOs.AdminDashboard;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminDashboards
{
    
  public class VendorContractExpiryHandler : IRequestHandler<VendorContractExpiryCommand, List<AVendorContractExpiryDto>>
    {
        private readonly IAdminDashboardRepository _repo;
        private readonly ILogger<VendorContractExpiryCommand> _logger;
        private readonly string _siteUrl;

        public VendorContractExpiryHandler(
            IAdminDashboardRepository repo,
            ILogger<VendorContractExpiryCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<AVendorContractExpiryDto>> Handle(VendorContractExpiryCommand request, CancellationToken cancellationToken)
        {
            // Fetch customer from repository
            var vendorMasterList = await _repo.GetAllVendorContractAsync(cancellationToken);

            // Map customerDto
            var customersDto = vendorMasterList.Select(vmaster => new AVendorContractExpiryDto
            {
                VendorName = vmaster.VendorName,
                ContractDocument =                 
              !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(vmaster.ContractDocument)
                ? $"{_siteUrl.TrimEnd('/')}/VendorContractDoc/{vmaster.ContractDocument.TrimStart('/')}"
                : null,

                ExpireOn = vmaster.ExpireOn,
                ContractId = vmaster.ContractId,
            }).ToList();

            return customersDto;
        }
    }
}
