using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NanoidDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class VendorContractRepository : IVendorContractRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<VendorContractRepository> _logger;
        private const string Prefix = "B2BV";
        private const int IdLength = 8;

        public VendorContractRepository(AOpsDbContext context, ILogger<VendorContractRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> AddAsync(VendorContract vcontract, CancellationToken cancellationToken = default)
        {
            await _context.VendorContract.AddAsync(vcontract, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<VendorContract>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VendorContract
                .AsNoTracking()
                .Include(vc => vc.Vendor) // optional, only if you want to include related vendor data
                .ToListAsync(cancellationToken);
        }

        public async Task<VendorContract?> GetByIdAsync(string contract_id, CancellationToken cancellationToken = default)
        {
            return await _context.VendorContract
                .AsNoTracking()
                .Include(vc => vc.Vendor) // optional
                .FirstOrDefaultAsync(vc => vc.ContractId == contract_id, cancellationToken);
        }

        public async Task<int> UpdateContractAsync(VendorContract vcontract, CancellationToken cancellationToken = default)
        {
            var vendorcontract = await _context.VendorContract.FirstOrDefaultAsync(x => x.ContractId == vcontract.ContractId, cancellationToken);
            if (vendorcontract == null)
            {
                throw new KeyNotFoundException("Vendor not found.");
            }
            vendorcontract.StartDate = vcontract.StartDate;
            vendorcontract.EndDate = vcontract.EndDate;
            vendorcontract.ContractDetails = vcontract.ContractDetails;
            vendorcontract.ContractValue = vcontract.ContractValue;
            vendorcontract.StaffsUsed = vcontract.StaffsUsed;
            vendorcontract.ContractType = vcontract.ContractType;
            vendorcontract.StaffAgreed = vcontract.StaffAgreed;
            vendorcontract.VehiclesAgreed = vcontract.VehiclesAgreed;
            vendorcontract.VehiclesUsed = vcontract.VehiclesUsed;
            vendorcontract.VendorId = vendorcontract.VendorId;
            if (!string.IsNullOrWhiteSpace(vcontract.AgreementDocument))
            {
                vendorcontract.AgreementDocument = vcontract.AgreementDocument;
            }
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<VendorContractDropdownDto>> GetContractDropdownAsync(Guid VendorId, CancellationToken cancellationToken = default)
        {
            return await _context.VendorContract
                .AsNoTracking().Where(vc => vc.VendorId == VendorId)
                .Select(c => new VendorContractDropdownDto
                {
                    VContractId = c.ContractId // or use c.ContractCode, or format custom string
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<string> ContractCodeGenerator(Guid vendorId, CancellationToken cancellationToken = default)
        {
            var vendor = await _context.VendorMaster
                 .Where(v => v.VendorId == vendorId)
                 .Select(v => new { v.Id }) // Assuming VendorCode is int
                 .FirstOrDefaultAsync(cancellationToken);

            if (vendor == null)
                throw new Exception($"Vendor not found for Id: {vendorId}");

            string nanoId = Nanoid.Generate(size: IdLength);
            return $"{Prefix}{vendor.Id}/{nanoId}";

        }

    }
}
