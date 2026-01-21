using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VendorContracts
{
    public class VendorContactDropdownHandler : IRequestHandler<VendorContactDropdownCommand, List<VendorContractDropdownDto>>
    {
        private IVendorContractRepository _repo;
        private ILogger<VendorContactDropdownCommand> _logger;

        public VendorContactDropdownHandler(IVendorContractRepository repo, ILogger<VendorContactDropdownCommand> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<List<VendorContractDropdownDto>> Handle(VendorContactDropdownCommand request, CancellationToken cancellationToken)
        {
            var vendorcontract = await _repo.GetContractDropdownAsync(request.VendorId, cancellationToken);
            return vendorcontract.Select(vcontact => new VendorContractDropdownDto
            {
                VContractId = vcontact.VContractId
            }).ToList();
        }
    }
}
