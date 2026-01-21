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
    public class GetVendorContractByIdHandler :IRequestHandler<GetVendorContractByIdCommand,DTOs.Vendor.GetVendorContractDto>
    {
        private IVendorContractRepository _vendorContractRepository;
        private ILogger<GetVendorContractByIdCommand> _logger;
        private readonly string _siteUrl;

        public GetVendorContractByIdHandler(IVendorContractRepository vendorcontract, ILogger<GetVendorContractByIdCommand> logger, string siteUrl)
        {
            _vendorContractRepository = vendorcontract;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<DTOs.Vendor.GetVendorContractDto> Handle(GetVendorContractByIdCommand request, CancellationToken cancellationToken)
        {
            var vcontact = await _vendorContractRepository.GetByIdAsync(request.contractId, cancellationToken);
            if (vcontact == null)
            {
                return null; // or throw an exception if preferred
            }
            return  new DTOs.Vendor.GetVendorContractDto()
            {
                VendorId = vcontact.VendorId,
                ContractDetails = vcontact.ContractDetails,
                ContractId = vcontact.ContractId,
                ContractType = vcontact.ContractType,
                ContractValue = vcontact.ContractValue,
                CreatedAt = vcontact.CreatedAt,
                StartDate = vcontact.StartDate,
                EndDate = vcontact.EndDate,
                VehiclesAgreed = vcontact.VehiclesAgreed,
                VehiclesUsed = vcontact.VehiclesUsed,
                StaffAgreed = vcontact.StaffAgreed,
                StaffUsed = vcontact.StaffsUsed,
                VendorName = vcontact.Vendor.VendorName,
                AgreementDocument =
              !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(vcontact.AgreementDocument)
                ? $"{_siteUrl.TrimEnd('/')}/VendorContractDoc/{vcontact.AgreementDocument.TrimStart('/')}"
                : null
            };
        }
    }
}
