using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vendor
{
    public class EditVendorContractDto
    {
       
        [Required]
        public Guid VendorId { get; set; }  

        [Required]
        [MaxLength(250)]
        public string ContractDetails { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ContractValue { get; set; }

        public IFormFile? AgreementDocument { get; set; }

        public string AgreementDocumentPath { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int VehiclesAgreed { get; set; }

        [Range(0, int.MaxValue)]
        public int VehiclesUsed { get; set; }

        [Range(0, int.MaxValue)]
        public int StaffAgreed { get; set; }

        [Range(0, int.MaxValue)]
        public int StaffUsed { get; set; }

        [MaxLength(50)]
        public string ContractType { get; set; } = string.Empty;

        public string ContractId { get; set; }

        public List<VendorDropdownDto> VendorList { get; set; } = new();
    }

}
