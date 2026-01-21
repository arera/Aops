using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vendor
{
    public class GetVendorDto
    {
        public Guid VendorId { get; set; }  

        public string VendorName { get; set; } = string.Empty;
        public string VendorMobile { get; set; } = string.Empty;
        public string VendorEmail { get; set; } = string.Empty;
        public string VendorAddress { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }  
        public bool isActive { get; set; }   
    }
}
