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
    public class VehicleDocumentRepository : IVehicleDocumentRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<VehicleDocumentRepository> _logger;

        public VehicleDocumentRepository(AOpsDbContext context, ILogger<VehicleDocumentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(VehicleDocument vehicle, CancellationToken cancellationToken = default)
        {
            await _context.VehicleDocument.AddAsync(vehicle
                , cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<IEnumerable<VehicleDocument>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VehicleDocument
                .Include(v => v.Vehicle)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> DeleteByAsync(Guid DocumentId, CancellationToken cancellationToken = default)
        {
            var document = await _context.VehicleDocument
                .FirstOrDefaultAsync(v => v.VehicleDocumentId == DocumentId, cancellationToken);

            if (document == null)
                return false;

            _context.VehicleDocument.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<VehicleDocument?> GetByIdAsync(Guid DocumentId, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleDocument
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehicleDocumentId == DocumentId, cancellationToken);
        }

        public async Task<int> UpdateVehicleAsync(VehicleDocument vehicles, CancellationToken cancellationToken = default)
        {
            var vehicle = await _context.VehicleDocument.FirstOrDefaultAsync(x => x.VehicleDocumentId == vehicles.VehicleDocumentId, cancellationToken);
            if (vehicle == null)
            {
                throw new KeyNotFoundException("Document not found.");
            }

            // Update fields
            vehicle.ExpiryDate = vehicles.ExpiryDate;
            vehicle.UpdatedAt = DateTime.UtcNow;
            vehicle.DocumentType = vehicles.DocumentType;
            vehicle.VehicleId = vehicles.VehicleId;
            if (!string.IsNullOrWhiteSpace(vehicles.FilePath))
            {
                vehicle.FilePath = vehicles.FilePath;
            }
            return await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
