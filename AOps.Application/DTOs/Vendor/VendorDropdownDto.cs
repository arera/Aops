using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vendor
{
    public class VendorDropdownDto
    { 
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
    }
}
