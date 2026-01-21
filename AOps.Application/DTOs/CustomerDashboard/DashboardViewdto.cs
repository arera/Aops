using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerDashboard
{
    public class DashboardViewdto
    {
        public List<VehiclesDto> Vehicles { get; set; }
        public List<EmployeesDto> Employees { get; set; }

    }
}
