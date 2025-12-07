using AOps.Domain.Entities.Common;
using AOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class CustomerTickets : BaseEntity
    {
        public Guid TicketId { get; set;}
        public Guid SiteId { get; set;}
        public Guid SubjectId { get; set;}
        public TicketPriority TicketPriority { get; set;}
        public TicketStatus TicketStatus { get; set;}
        public string? TicketDescription { get; set;}
        public TicketCategorys TicketCategory { get; set;}
        public int TicketIssueType { get;set;}
        public string? ResolutionNote { get; set; }
        public DateTime? ClosedAt { get; set; }

        public Guid HandledBy { get; set;}

        public virtual CustomerSite Site { get; set; }
        public virtual VehicleMaster? Vehicle { get; set; }
        public virtual EmployeeMaster? Employee { get; set; }
        public virtual Orglevels? Executive { get; set; }

        public void SetIssueType(Enum issueType)
        {
            if (TicketCategory == TicketCategorys.Vehicle && issueType is VehicleTicketIssueType)
            {
                TicketIssueType = (int)(VehicleTicketIssueType)issueType;
            }
            else if (TicketCategory == TicketCategorys.Manpower && issueType is ManpowerTicketIssueType)
            {
                TicketIssueType = (int)(ManpowerTicketIssueType)issueType;
            }
            else
            {
                throw new InvalidOperationException($"IssueType {issueType} does not match TicketCategory {TicketCategory}");
            }
        }
    }
}
