using AOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerTickets
{
    public class GetCustomerTicketDto
    {
        public Guid TicketId {get;set;}
        public String SiteName { get; set; }
        public String SubjectName { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public string TicketSource { get; set; }
        public string TicketDescription { get; set; }
        public TicketCategorys TicketCategory { get; set; }
        public string TicketIssueType { get; set; }
        public string CurrentStatus { get; set; }
        public int TStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TicketNumber { get; set; }
    }
}
