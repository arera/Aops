using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AdminDashboard
{
    public class ADashboardViewDto
    {
        public List<ACustomerContractExpiryDto> CustomerContracts { get; set; } = new List<ACustomerContractExpiryDto>();
        public List<AVendorContractExpiryDto> VendorContracts { get; set; } = new List<AVendorContractExpiryDto>();
        public List<VehicleDocumentExpiryDto> VehicleDocuments { get; set; } = new List<VehicleDocumentExpiryDto>();
    }
}
