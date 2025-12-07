using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class CustomerLoginRepository : ICustomerLoginRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CustomerLoginRepository> _logger;
    
     public CustomerLoginRepository(AOpsDbContext context, ILogger<CustomerLoginRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(CustomerLogin login)
        {
            try
            {
                await _context.CustomerLogin.AddAsync(login);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding CustomerLogin for CustomerId {CustomerId}", login.CustomerId);
                throw;
            }
        }

        /// <summary>
        /// Change the customer's password.
        /// </summary>
        public async Task ChangePasswordAsync(Guid customerId, string newPasswordHash,string ipaddress)
        {
            try
            {
                var login = await _context.CustomerLogin
                                          .FirstOrDefaultAsync(cl => cl.CustomerId == customerId);

                if (login == null)
                    throw new InvalidOperationException("Customer login not found.");

                login.PasswordHash = newPasswordHash;
                login.IpAddress = ipaddress;
                login.LastLogin = DateTime.UtcNow;
                _context.CustomerLogin.Update(login);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while changing password for CustomerId {CustomerId}", customerId);
                throw;
            }
        }

        /// <summary>
        /// Retrieve login record by CustomerId.
        /// </summary>
        public async Task<CustomerLogin?> GetByCustomerEmailAsync(string email)
        {
            try
            {
                return await _context.CustomerLogin
                    .AsNoTracking()
                    .Join(_context.Customers,
                          login => login.CustomerId,
                          customer => customer.UserId,
                          (login, customer) => new { login, customer })
                    .Where(x => x.customer.Email == email)
                    .Select(x => x.login)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching login record for Email {Email}", email);
                throw;
            }
        }

        public async Task<CustomerLogin?> GetByUserIdAsync(Guid customerId)
        {
            try
            {
                return await _context.CustomerLogin.FirstOrDefaultAsync(cl => cl.CustomerId == customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching login record for Customer {customerId}", customerId);
                throw;
            }
        }


        /// <summary>
        /// Validate login credentials and update last login/IP if valid.
        /// </summary>
        public async Task<bool> ValidateLoginAsync(Guid customerId, string passwordHash, string ipAddress)
        {
            try
            {
                var login = await _context.CustomerLogin.FirstOrDefaultAsync(cl => cl.CustomerId == customerId);

                if (login == null)
                    return false;

                // ⚠️ In production, hash/verify the password securely (e.g., BCrypt, PBKDF2, or ASP.NET PasswordHasher)
                if (login.PasswordHash != passwordHash)
                    return false;

                login.LastLogin = DateTime.UtcNow;
                login.IpAddress = ipAddress;

                _context.CustomerLogin.Update(login);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while validating login for CustomerId {CustomerId}", customerId);
                throw;
            }
        }
    }
}
   
