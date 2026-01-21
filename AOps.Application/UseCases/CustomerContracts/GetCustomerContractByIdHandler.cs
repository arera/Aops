using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerContracts
{
    public class GetCustomerContractByIdHandler :IRequestHandler<GetCustomerContractByIdCommand, DTOs.CustomerContracts.GetCustomerContractDto>
    {
        private ICustomerContractRepository _repo;
        private ILogger<GetCustomerContractByIdHandler> _logger;
        private readonly string _siteUrl;

        public GetCustomerContractByIdHandler(ICustomerContractRepository repo, ILogger<GetCustomerContractByIdHandler> logger, string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<DTOs.CustomerContracts.GetCustomerContractDto> Handle(GetCustomerContractByIdCommand request, CancellationToken cancellationToken)
        {
            var ccontact = await _repo.GetByIdAsync(request.contractId, cancellationToken);
            if (ccontact == null)
            {
                return null; // or throw an exception if preferred
            }
            return  new DTOs.CustomerContracts.GetCustomerContractDto()
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
            };
        }
    }
}
