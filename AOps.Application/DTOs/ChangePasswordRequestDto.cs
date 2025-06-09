using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs
{
    // ChangePasswordRequestDto.cs
    public class ChangePasswordRequestDto
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
