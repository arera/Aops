using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vendor
{
        public class AddVendorContractDto
    {
        [Required]
        public Guid VendorId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContractCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(5000)]
        public string ContractDetails { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ContractValue { get; set; }

        public string AgreementDocument { get; set; } = string.Empty;

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
    }

}
