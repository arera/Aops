using AOps.Application.Interfaces;
using AOps.Application.UseCases.Vendor;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.CustomerContracts
{
    public class GetAllCustomerContractHandler : IRequestHandler<GetAllCustomerContractCommand, List<DTOs.CustomerContracts.GetCustomerContractDto>>
    {
        private ICustomerContractRepository _repo;
        private ILogger<GetAllCustomerContractHandler> _logger;
        private readonly string _siteUrl;

        public GetAllCustomerContractHandler(ICustomerContractRepository repo, ILogger<GetAllCustomerContractHandler> logger, string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<List<DTOs.CustomerContracts.GetCustomerContractDto>> Handle(GetAllCustomerContractCommand request, CancellationToken cancellationToken)
        {
            var customercontract = await _repo.GetAllAsync(cancellationToken);
            return customercontract.Select(ccontact => new DTOs.CustomerContracts.GetCustomerContractDto
            {
                CustomerId = ccontact.CustomerId,
                ContractDetails = ccontact.ContractDetails,
                ContractId = ccontact.ContractId,
                ContractType = ccontact.ContractType,
                ContractValue = ccontact.ContractValue,
                CreatedAt = ccontact.CreatedAt,
                StartDate = ccontact.StartDate,
                EndDate = ccontact.EndDate,
                VehiclesAgreed = ccontact.VehiclesAgreed,
                VehiclesUsed = ccontact.VehiclesUsed,
                StaffAgreed = ccontact.StaffAgreed,
                StaffUsed = ccontact.StaffsUsed,
                CustomerName = ccontact.Customer.Name,
                AgreementDocument =
              !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(ccontact.AgreementDocument)
                ? $"{_siteUrl.TrimEnd('/')}/CustomerContractDoc/{ccontact.AgreementDocument.TrimStart('/')}"
                : null
            }).ToList();
        }
    }
}
