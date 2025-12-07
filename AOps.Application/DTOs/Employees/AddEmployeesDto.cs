using AOps.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Employees
{
    public class AddEmployeesDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        public Designation Designation { get; set; }
        public EmploymentType EmploymentType { get; set; }

        public string? GovernmentId { get; set; }
        public string? ProfessionalId { get; set; }
        public IFormFile? GovDocumentFile { get; set; }
        public IFormFile? ProfesionalDocumentFile { get; set; }
    }
}
