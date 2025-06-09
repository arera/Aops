using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    }
}
