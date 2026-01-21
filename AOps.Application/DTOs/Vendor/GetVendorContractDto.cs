using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vendor
{
       public class GetVendorContractDto
        {
            public int Id { get; set; }  
            public string ContractId { get; set; } = string.Empty;

            public Guid VendorId { get; set; }
            public string VendorName { get; set; } = string.Empty;

            public string ContractDetails { get; set; } = string.Empty;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal ContractValue { get; set; }

            public string AgreementDocument { get; set; } = string.Empty;

            public int VehiclesAgreed { get; set; }
            public int VehiclesUsed { get; set; }

            public int StaffAgreed { get; set; }
            public int StaffUsed { get; set; }

            public string ContractType { get; set; } = string.Empty;

            public DateTime CreatedAt { get; set; }
        }

    }

