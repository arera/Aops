using AOps.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class SiteEmployeeAssignment :BaseEntity
    {
        public Guid SiteId { get; set; }
        public CustomerSite CustomerSite { get; set; }
        public Guid EmployeeId { get; set; }
        public EmployeeMaster Employee { get; set; }
    }
}
