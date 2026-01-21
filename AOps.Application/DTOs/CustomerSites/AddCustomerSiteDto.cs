using AOps.Application.DTOs.Customer;
using AOps.Application.DTOs.CustomerContracts;
using AOps.Application.DTOs.DropDown;
using AOps.Application.DTOs.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerSites
{
    public class AddCustomerSiteDto
    {
        public Guid CustomerId { get; set; }
        public string SiteName { get; set; }
        public string SiteCity { get; set; }
        public string SiteState { get; set; }
        public string SiteZip { get; set; }
        public string SiteCountry { get; set; }

        public string CustomerContractId { get; set;}

        public List<Guid> AssignedVehicleIds { get; set; } = new();

        public List<Guid> AssignedmployeeIds { get; set; } = new();

        public List<CustomerDropdownDto> CustomerList { get; set; } = new();
        public List<CustomerContractDropdown> CustomerContract { get; set; } = new();

        public List<VehicleDropdownDto> VehicleList { get; set; } = new();

        public List<EmployeeSelectDto> EmployeeSelectList { get; set; } = new();
    }

}
