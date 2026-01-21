using AOps.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Employees
{
    public class EditEmployeesDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public bool EStatus { get; set; }

        [Required]
        public Designation Designation { get; set; }

        [Required]
        public EmploymentType EmploymentType { get; set; }

        // Existing stored file paths
        public string? GovernmentId { get; set; }
        public string? ProfessionalId { get; set; }

        // New uploads
        public IFormFile? GovDocumentFile { get; set; }
        public IFormFile? ProfessionalDocumentFile { get; set; }
    }

}
