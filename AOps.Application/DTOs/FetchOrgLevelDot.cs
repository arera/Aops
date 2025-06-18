using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs
{
    public class FetchOrgLevelDot
    {
        public Guid UserID { get; set; } = new Guid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
        public string? Mobile { get; set; }
        public bool IsActive { get; set; } 
    }
}
