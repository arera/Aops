using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AdminDashboard
{
    public class ATotalCountDto
    {
        public int CustomerCount { get; set;}
        public int VendorCount { get; set;}
        public int SiteCount { get; set;}
        public int EmployeeCount { get; set;}
    }
}
