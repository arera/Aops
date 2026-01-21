using AOps.Application.DTOs.DropDown;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.EmpContract
{
    public class FetchAllEmployeeContractDto
    {
        [Required]
        public string EmployeeName { get; set; }
        public string ContractID { get; set; }
        [Required]
        public DateTime ContractStartDate { get; set; }
        [Required]
        public DateTime ContractEndDate { get; set; }
        public string Description { get; set; }
        public string? ContractDocument { get; set; }
        
    }
}
