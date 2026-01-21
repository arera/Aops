using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IVehicleDocumentRepository
    {
        Task<int> AddAsync(VehicleDocument vehicledoc, CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleDocument>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteByAsync(Guid DocumentId, CancellationToken cancellationToken = default);
        Task<VehicleDocument?> GetByIdAsync(Guid DocumentId, CancellationToken cancellationToken = default);
        Task<int> UpdateVehicleAsync(VehicleDocument vehicledocument, CancellationToken cancellationToken = default);
    }
}
