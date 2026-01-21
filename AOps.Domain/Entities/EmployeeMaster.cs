using AOps.Domain.Entities.Common;
using AOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class EmployeeMaster : BaseEntity
    {
        public Guid EmployeeId { get; set;}
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public Designation DesignationId { get; set; }  
        public EmploymentType EmploymentTypeId { get; set; } 
        public string? GovernmentId { get; set; }
        public string? ProfessionalId { get; set; }
    }
}
