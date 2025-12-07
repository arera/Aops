using AOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AdminServiceTickets
{
    public class FetchAllTicketsDto
    {
        public Guid TicketId { get; set; }
        public String SiteName { get; set; }
        public String SubjectName { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public string TicketDescription { get; set; }
        public TicketCategorys TicketCategory { get; set; }
        public string TicketIssueType { get; set; }
        public TicketStatus CurrentStatus { get; set; }
        public int TStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string HandelBy{ get; set; }
        public string ResolutionNote { get;set; }
        public int TicketNumber { get; set; }
    }
}
