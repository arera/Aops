using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vendor
{
    public class EditVendorDto
    {
        [Required]
        public int Id { get; set; }  // Required for editing

        [Required]
        [MaxLength(100)]
        public string VendorName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string VendorMobile { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string VendorEmail { get; set; } = string.Empty;

        [MaxLength(250)]
        public string VendorAddress { get; set; } = string.Empty;
    }

}
