using AOps.Application.DTOs.AdminDashboard;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;


namespace AOps.Application.UseCases.AdminDashboards
{
    public class CustomerContractExpiryHandler: IRequestHandler<CustomerContractExpiryCommand, List<ACustomerContractExpiryDto>>
    {
        private readonly IAdminDashboardRepository _repo;
        private readonly ILogger<CustomerContractExpiryCommand> _logger;
        private readonly string _siteUrl;

        public CustomerContractExpiryHandler(
            IAdminDashboardRepository repo,
            ILogger<CustomerContractExpiryCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<ACustomerContractExpiryDto>> Handle(CustomerContractExpiryCommand request,CancellationToken cancellationToken)
        {
            // Fetch customer from repository
            var customerMasterList = await _repo.GetAllCustomerContractAsync(cancellationToken);

            // Map customerDto
            var customersDto = customerMasterList.Select(vmaster => new ACustomerContractExpiryDto
            {
                CustomerName = vmaster.CustomerName,
                ContractDocument = 
                !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(vmaster.ContractDocument)
                ? $"{_siteUrl.TrimEnd('/')}/CustomerContractDoc/{vmaster.ContractDocument.TrimStart('/')}"
                : null,
                ExpireOn = vmaster.ExpireOn,
                ContractId = vmaster.ContractId,
            }).ToList();

            return customersDto;
        }
    }
}
