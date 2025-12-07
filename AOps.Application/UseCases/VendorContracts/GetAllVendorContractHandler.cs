using AOps.Application.Interfaces;
using AOps.Application.UseCases.Vendor;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.VendorContracts
{
    public class GetAllVendorContractHandler : IRequestHandler<GetAllVendorContractCommand, List<DTOs.Vendor.GetVendorContractDto>>
    {
        private IVendorContractRepository _vendorContractRepository;
        private ILogger<GetAllVendorContractCommand> _logger;
        private readonly string _siteUrl;

        public GetAllVendorContractHandler(IVendorContractRepository vendorcontract, ILogger<GetAllVendorContractCommand> logger, string siteUrl)
        {
            _vendorContractRepository = vendorcontract;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<List<DTOs.Vendor.GetVendorContractDto>> Handle(GetAllVendorContractCommand request, CancellationToken cancellationToken)
        {
            var vendorcontract = await _vendorContractRepository.GetAllAsync(cancellationToken);
            return vendorcontract.Select(vcontact => new DTOs.Vendor.GetVendorContractDto
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
            }).ToList();
        }
    }
}
