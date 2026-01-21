using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerLogins
{
    public class CustomerLoginResponseDto
    {
        public Guid CustomerId { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Success { get; set; }
        public int? RoleId { get; set; }
        public string? Message { get; set; }  
    }
}
