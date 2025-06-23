using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid UserId { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
       
    }
}
