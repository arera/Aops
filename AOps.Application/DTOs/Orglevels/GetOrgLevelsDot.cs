using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs
{
    public class GetOrgLevelsDot
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public int Role { get; set; }
        public Guid UserID { get; set; }
        public string? Mobile { get; set; }

        public string RoleText => Role switch
        {
            1 => "Admin",
            2 => "Manager",
            3 => "User",
            _ => "Unknown"
        };

        public bool IsActive { get; set; }
    }
}
