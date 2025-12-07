using AOps.Application.DTOs.Vehicle;
using AOps.Application.DTOs.Vendor;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task<int> AddAsync(VehicleMaster vendor, CancellationToken cancellationToken = default);
        Task<bool> ExistsVehicleAsync(string vehicle_number, CancellationToken cancellationToken = default);
        Task<bool> ExistsVehicleAsync(string vehicle_number, Guid VehicleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleMaster>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<VehicleDropdownDto>> GetVehicleDropdownAsync(CancellationToken cancellationToken = default);

        Task<VehicleMaster?> GetByIdAsync(Guid vendorId, CancellationToken cancellationToken = default);
        Task<int> UpdateVehicleAsync(VehicleMaster vendor, CancellationToken cancellationToken = default);

    }
}
