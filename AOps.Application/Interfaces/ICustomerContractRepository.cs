using AOps.Application.DTOs.CustomerContracts;
using AOps.Application.DTOs.Vendor;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICustomerContractRepository
    {
        Task<int> AddAsync(CustomerContract ccontract, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomerContract>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CustomerContract?> GetByIdAsync(string contract_id, CancellationToken cancellationToken = default);
        Task<int> UpdateContractAsync(CustomerContract ccontract, CancellationToken cancellationToken = default);
        Task<List<CustomerContractDropdown>> GetContractDropdownAsync(Guid CustomerId, CancellationToken cancellationToken = default);
        Task<string> ContractCodeGenerator(Guid customerId, CancellationToken cancellationToken = default);

    }
}
