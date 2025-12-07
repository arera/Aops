using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AOps.Infrastructure.Persistence
{
    public class VendorRepository : IVendorRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<VendorRepository> _logger;

        public VendorRepository(AOpsDbContext context, ILogger<VendorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(VendorMaster vendor, CancellationToken cancellationToken = default)
        {
            await _context.VendorMaster.AddAsync(vendor, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsMobileAsync(string mobile, CancellationToken cancellationToken = default)
        {
            return await _context.VendorMaster.AnyAsync(v => v.VendorMobile == mobile, cancellationToken);
        }

        public async Task<bool> ExistsMobileAsync(string mobile, Guid vendorId, CancellationToken cancellationToken = default)
        {
            return await _context.VendorMaster
                .AnyAsync(v => v.VendorMobile == mobile && v.VendorId != vendorId, cancellationToken);
        }

        public async Task<IEnumerable<VendorMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VendorMaster
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<VendorMaster?> GetByIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
        {
            return await _context.VendorMaster
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VendorId == vendorId, cancellationToken);
        }

        public async Task<int> UpdateVendorAsync(VendorMaster vendors, CancellationToken cancellationToken = default)
        {
            var vendor = await _context.VendorMaster.FirstOrDefaultAsync(x => x.VendorId == vendors.VendorId, cancellationToken);
            if (vendor == null)
            {
                throw new KeyNotFoundException("Vendor not found.");
            }

            // Update fields
            vendor.VendorName = vendors.VendorName;
            vendor.VendorMobile = vendors.VendorMobile;
            vendor.VendorAddress = vendors.VendorAddress;
            vendor.VendorEmail = vendors.VendorEmail;
            vendor.UpdatedAt = DateTime.UtcNow;
            vendor.IsDeleted = vendors.IsDeleted; // Flip if IsActive is passed

             return await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task<List<VendorDropdownDto>> GetVendorDropdownAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VendorMaster
                .AsNoTracking()
                .Select(v => new VendorDropdownDto
                {
                    VendorId = v.VendorId,
                    VendorName = v.VendorName
                })
                .ToListAsync(cancellationToken);
        }

    }
}
