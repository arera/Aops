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
    public class GetCustomerContractDropdownHandler : IRequestHandler<GetCustomerContractDropdownCommand, List<DTOs.CustomerContracts.CustomerContractDropdown>>
    {
        private ICustomerContractRepository _repo;
        private ILogger<GetAllCustomerContractHandler> _logger;
        private readonly string _siteUrl;

        public GetCustomerContractDropdownHandler(ICustomerContractRepository repo, ILogger<GetAllCustomerContractHandler> logger, string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }
        public async Task<List<DTOs.CustomerContracts.CustomerContractDropdown>> Handle(GetCustomerContractDropdownCommand request, CancellationToken cancellationToken)
        {
            var customercontract = await _repo.GetContractDropdownAsync(request.CustomerId, cancellationToken);
            return customercontract.Select(ccontact => new DTOs.CustomerContracts.CustomerContractDropdown
            {
                Ccontractid = ccontact.Ccontractid,
            }).ToList();
        }
    }
    }
