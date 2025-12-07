using AOps.Application.DTOs.Vendor;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IVendorRepository
    {
        Task<int> AddAsync(VendorMaster vendor, CancellationToken cancellationToken = default);
        Task<bool> ExistsMobileAsync(string mobile, CancellationToken cancellationToken = default);
        Task<bool> ExistsMobileAsync(string mobile, Guid vendorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<VendorMaster>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<VendorDropdownDto>> GetVendorDropdownAsync(CancellationToken cancellationToken = default);

        Task<VendorMaster?> GetByIdAsync(Guid vendorId, CancellationToken cancellationToken = default);
        Task<int> UpdateVendorAsync(VendorMaster vendor, CancellationToken cancellationToken = default);
    }
}
