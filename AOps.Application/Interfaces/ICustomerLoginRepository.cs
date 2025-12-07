using AOps.Application.DTOs.CustomerLogins;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICustomerLoginRepository
    {
        Task AddAsync(CustomerLogin login);

        Task ChangePasswordAsync(Guid customerId, string newPasswordHash,string ipaddress);

        Task<CustomerLogin?> GetByCustomerEmailAsync(string email_id);

        Task<CustomerLogin?> GetByUserIdAsync(Guid customerId);

        Task<bool> ValidateLoginAsync(Guid customerId, string passwordHash, string ipAddress);
    }

}
