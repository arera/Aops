using AOps.Application.DTOs.DropDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerSites
{
    public class GetCustomerSiteDto
    {
        public Guid SiteId { get; set; }
        public Guid CustomerId { get; set; }

        public string SiteName { get; set; }
        public string SiteCity { get; set; }
        public string SiteState { get; set; }
        public string SiteZip { get; set; }
        public string SiteCountry { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContract { get; set; }

        public List<string> Vehicle_number_list { get; set; } = new(); // ✅ new property
        public List<Guid> AssignedVehicleIds { get; set; } = new(); // ✅ new property

        public List<string> Employee_name_list { get; set; } = new(); // ✅ new property
        public List<Guid> AssignedEmployeeIds { get; set; } = new(); // ✅ new property


    }

}
