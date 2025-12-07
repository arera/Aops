using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vehicle
{
    public class FetchVehicleDto
    {
        public Guid VehicleId { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;

        public string VehicleNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public string VehicleFuelType { get; set; } = string.Empty;
        public string AmbulanceType { get; set; } = string.Empty;
        public string VendorContractId {  get; set; } = string.Empty;
        public DateTime Registered_on { get; set; }

        public string? RegistrationDocumentPath { get; set; } // For display or download
    }
}
