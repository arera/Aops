using AOps.Application.DTOs.Vendor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vehicle
{
    public class EditVehicleDto
    {
        public Guid VehicleId { get; set; }  // For update scenario
        public Guid VendorId { get; set; }

        public string VehicleNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public string VehicleFuelType { get; set; } = string.Empty;
        public string AmbulanceType { get; set; } = string.Empty;

        public string? VContractId { get; set;} = string.Empty;

        public IFormFile? RegistrationDocumentFile { get; set; }  // For re-upload
        public string? ExistingDocumentPath { get; set; }         // Show current doc name

        public List<VendorDropdownDto> VendorList { get; set; } = new();
        public List<VendorContractDropdownDto> VContractList { get; set; } = new();
    }

}
