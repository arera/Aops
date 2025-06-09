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

        public async Task<int> AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Customers.AddAsync(customer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return customer.Id;
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

    }
}
