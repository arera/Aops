using AOps.Application.DTOs.Vehicle;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IEmployeeMasterRepository
    {
        Task<int> AddAsync(EmployeeMaster Emp, CancellationToken cancellationToken = default);
        Task<bool> ExistsEmployeeAsync(string mobile_number, CancellationToken cancellationToken = default);
        Task<bool> ExistsEmployeeAsync(string mobile_number, Guid EmployeeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EmployeeMaster>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<EmployeeMaster?> GetByIdAsync(Guid EmployeeId, CancellationToken cancellationToken = default);
        Task<int> UpdateEmployeeAsync(EmployeeMaster employee, CancellationToken cancellationToken = default);

    }
}
