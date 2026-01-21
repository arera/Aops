using AOps.Application.DTOs.Vendor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vehicle
{
         public class AddVehicleDto
        {
            [Required]
            public Guid VendorId { get; set; }
             [Required]
            public string VehicleNumber { get; set; } = string.Empty;
            [Required]
            public string VehicleType { get; set; } = string.Empty;
            [Required]
            public string VehicleModel { get; set; } = string.Empty;
            public string VehicleFuelType { get; set; } = string.Empty;
            public string AmbulanceType { get; set; } = string.Empty;
           public IFormFile? RegistrationDocumentFile { get; set; }

        public List<VendorDropdownDto> VendorList { get; set; } = new ();
       
        }
    }

