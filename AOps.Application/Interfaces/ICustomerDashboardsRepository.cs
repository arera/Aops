using AOps.Application.DTOs.CustomerDashboard;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICustomerDashboardsRepository
    {
        Task<List<VehiclesDto>> GetAllVehicleAsync(Guid CustomerId, CancellationToken cancellationToken = default);
        Task<List<EmployeesDto>> GetAllEmployeeAsync(Guid CustomerId, CancellationToken cancellationToken = default);
    }
}
