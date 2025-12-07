using AOps.Application.DTOs.Customer;
using AOps.Application.DTOs.DropDown;
using AOps.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerTickets
{
    public class CreateTicketDto
    {
        public Guid SiteId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? EmployeeId { get; set; }  // add this

        public TicketPriority TicketPriority { get; set; }
        public string TicketDescription { get; set; }
        public TicketCategorys TicketCategory { get; set; }
        public string TicketIssueType { get; set; }
        public VehicleTicketIssueType? VehicleIssueType { get; set; }
        public ManpowerTicketIssueType? ManpowerIssueType { get; set; }


        public List<SiteSelectDto> SiteList { get; set; } = new();
        public List<VehicleSelectDto> VehicleList { get; set; } = new();
        public List<EmployeeSelectDto> EmployeeList { get; set; } = new();

        public List<SelectListItem> VehicleIssueTypes { get; set; } = new();
        public List<SelectListItem> ManpowerIssueTypes { get; set; } = new();


    }
}
