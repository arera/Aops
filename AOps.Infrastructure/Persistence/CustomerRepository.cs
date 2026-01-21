using AOps.Application.DTOs.Customer;
using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Azure.Core.HttpHeader;

namespace AOps.Infrastructure.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(AOpsDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Customers.AddAsync(customer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return customer.UserId;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating customer with Email: {Email}", customer.Email);

                // Optional: You could check for specific constraint violations here
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating customer with Email: {Email}", customer.Email);
                throw;
            }
        }
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email, Guid UserId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email && c.UserId != UserId, cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(Guid UserId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.FirstOrDefaultAsync(u => u.UserId == UserId);
        }

        public async Task<int> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == customer.UserId, cancellationToken);

            if (existing == null)
            {
                throw new KeyNotFoundException($"User with ID {customer.UserId} not found.");
            }

            existing.Name = customer.Name;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.Email = customer.Email;
            existing.PrimaryMobile = customer.PrimaryMobile;
            existing.CreatedBy = customer.CreatedBy;
            existing.SecondaryMobile = customer.SecondaryMobile;
            existing.IsDeleted = customer.IsDeleted;
            existing.GST = customer.GST;
            existing.Address.ZipCode = customer.Address.ZipCode;
            existing.Address.Street = customer.Address.Street;
            existing.Address.City = customer.Address.City;
            existing.Address.State = customer.Address.State;
            existing.Address.Country = customer.Address.Country;
            // Update other properties as needed

            return await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task<List<CustomerDropdownDto>>GetCustomerDropdown(CancellationToken cancellationToken)
        {
            return await _context.Customers
                 .AsNoTracking()
                 .Select(v => new CustomerDropdownDto
                 {
                     CustomerId = v.UserId,
                     CustomerName = v.Name
                 })
                 .ToListAsync(cancellationToken);
        }
    }
}
