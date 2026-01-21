using AOps.Application.DTOs.CustomerContracts;
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
    public class CustomerContractRepository : ICustomerContractRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CustomerContractRepository> _logger;
        private const string Prefix = "C2VC";
        private const int IdLength = 8;

        public CustomerContractRepository(AOpsDbContext context, ILogger<CustomerContractRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(CustomerContract ccontract, CancellationToken cancellationToken = default)
        {
            await _context.CustomerContract.AddAsync(ccontract, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<CustomerContract>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.CustomerContract
                .AsNoTracking()
                .Include(vc => vc.Customer) // optional, only if you want to include related vendor data
                .ToListAsync(cancellationToken);
        }

        public async Task<CustomerContract?> GetByIdAsync(string contract_id, CancellationToken cancellationToken = default)
        {
            return await _context.CustomerContract
                .AsNoTracking()
                .Include(vc => vc.Customer) // optional
                .FirstOrDefaultAsync(vc => vc.ContractId == contract_id, cancellationToken);
        }

        public async Task<int> UpdateContractAsync(CustomerContract ccontract, CancellationToken cancellationToken = default)
        {
            var customercontract = await _context.CustomerContract.FirstOrDefaultAsync(x => x.ContractId == ccontract.ContractId, cancellationToken);
            if (customercontract == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }
            customercontract.StartDate = ccontract.StartDate;
            customercontract.EndDate = ccontract.EndDate;
            customercontract.ContractDetails = ccontract.ContractDetails;
            customercontract.ContractValue = ccontract.ContractValue;
            customercontract.StaffsUsed = ccontract.StaffsUsed;
            customercontract.ContractType = ccontract.ContractType;
            customercontract.StaffAgreed = ccontract.StaffAgreed;
            customercontract.VehiclesAgreed = ccontract.VehiclesAgreed;
            customercontract.VehiclesUsed = ccontract.VehiclesUsed;
            customercontract.CustomerId = ccontract.CustomerId;
            if (!string.IsNullOrWhiteSpace(ccontract.AgreementDocument))
            {
                customercontract.AgreementDocument = ccontract.AgreementDocument;
            }
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<CustomerContractDropdown>> GetContractDropdownAsync(Guid CustomerId, CancellationToken cancellationToken = default)
        {
            return await _context.CustomerContract
                .AsNoTracking().Where(vc => vc.CustomerId == CustomerId)
                .Select(c => new CustomerContractDropdown
                {
                    Ccontractid = c.ContractId // or use c.ContractCode, or format custom string
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<string> ContractCodeGenerator(Guid customerId, CancellationToken cancellationToken = default)
        {
            var customer = await _context.Customers
                 .Where(v => v.UserId == customerId)
                 .Select(v => new { v.Id }) // Assuming VendorCode is int
                 .FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                throw new Exception($"Customer not found for Id: {customerId}");

            string nanoId = Nanoid.Generate(size: IdLength);
            return $"{Prefix}{customer.Id}/{nanoId}";

        }

    }
}

