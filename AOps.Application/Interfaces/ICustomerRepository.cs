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
        Task<Guid> AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, Guid UserId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Customer?> GetByIdAsync(Guid Userid, CancellationToken cancellationToken = default);
        Task<int> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);

        Task<List<DTOs.Customer.CustomerDropdownDto>>GetCustomerDropdown(CancellationToken cancellationToken = default);


    }
}
