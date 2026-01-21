using AOps.Application.DTOs.Vendor;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IVendorContractRepository
    {
        Task<int> AddAsync(VendorContract vcontract, CancellationToken cancellationToken = default);
        Task<IEnumerable<VendorContract>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<VendorContract?> GetByIdAsync(string contract_id, CancellationToken cancellationToken = default);
        Task<int> UpdateContractAsync(VendorContract vcontract, CancellationToken cancellationToken = default);
        Task<List<VendorContractDropdownDto>> GetContractDropdownAsync(Guid VendorId, CancellationToken cancellationToken = default);
        Task<string> ContractCodeGenerator(Guid vendorId, CancellationToken cancellationToken = default);

    }
}
