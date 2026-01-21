using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerLogins
{
    public class CustomerChangePasswordDto
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime Updated_on {get; set; }
        public string? ipAddress {get; set; }
    }
}
