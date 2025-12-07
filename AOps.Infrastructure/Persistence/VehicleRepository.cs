using AOps.Application.DTOs.Vehicle;
using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<VendorRepository> _logger;

        public VehicleRepository(AOpsDbContext context, ILogger<VendorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(VehicleMaster vehicle, CancellationToken cancellationToken = default)
        {
            await _context.VehicleMaster.AddAsync(vehicle
                , cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> ExistsVehicleAsync(string vehicleNumber, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleMaster
                .AnyAsync(v => v.VehicleNumber.ToLower() == vehicleNumber.ToLower() && !v.IsDeleted, cancellationToken);
        }

        public async Task<bool> ExistsVehicleAsync(string VehicleNumber, Guid vehicleId, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleMaster
                .AnyAsync(v => v.VehicleNumber == VehicleNumber && v.VehicleId != vehicleId, cancellationToken);
        }
        public async Task<IEnumerable<VehicleMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VehicleMaster
                .Include(v => v.Vendor)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public async Task<VehicleMaster?> GetByIdAsync(Guid VehicleId, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleMaster
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehicleId == VehicleId, cancellationToken);
        }

        public async Task<int> UpdateVehicleAsync(VehicleMaster vehicles, CancellationToken cancellationToken = default)
        {
            var vehicle = await _context.VehicleMaster.FirstOrDefaultAsync(x => x.VehicleId == vehicles.VehicleId, cancellationToken);
            if (vehicle == null)
            {
                throw new KeyNotFoundException("Vehicle not found.");
            }

            // Update fields
            vehicle.VehicleNumber = vehicles.VehicleNumber;
            vehicle.UpdatedAt = DateTime.UtcNow;
            vehicle.VehicleFuelType = vehicles.VehicleFuelType;
            vehicle.VehicleModel = vehicles.VehicleModel;
            vehicle.VendorId = vehicles.VendorId;
            vehicle.AmbulanceType = vehicles.AmbulanceType;
            vehicle.VehicleType = vehicles.VehicleType;
            vehicle.VendorContractId = vehicles.VendorContractId;
            if (!string.IsNullOrWhiteSpace(vehicles.RegistrationDocument))
            {
                vehicle.RegistrationDocument = vehicles.RegistrationDocument;
            }
            return await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task<List<VehicleDropdownDto>> GetVehicleDropdownAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VehicleMaster
                  .AsNoTracking()
                  .Select(v => new VehicleDropdownDto
                  {
                      VehicleId = v.VehicleId,
                      VehicleNumber = v.VehicleNumber
                  })
                  .ToListAsync(cancellationToken);
   
        }
    }
}
