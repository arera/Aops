using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Vehicle
{
    public class VehicleDropdownDto
    {
        public Guid VehicleId { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
    }
}
