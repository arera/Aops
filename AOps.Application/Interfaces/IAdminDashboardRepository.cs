using AOps.Application.DTOs.AdminDashboard;
using AOps.Application.DTOs.CustomerDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<List<ACustomerContractExpiryDto>> GetAllCustomerContractAsync(CancellationToken cancellationToken = default);
        Task<List<AVendorContractExpiryDto>> GetAllVendorContractAsync(CancellationToken cancellationToken = default);
        Task<List<VehicleDocumentExpiryDto>> GetAllExpiredVehicleDocumentsAsync(CancellationToken cancellationToken = default);
    }
}
