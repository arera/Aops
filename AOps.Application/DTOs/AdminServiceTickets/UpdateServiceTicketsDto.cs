using AOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AdminServiceTickets
{
    public class UpdateServiceTicketsDto
    {
        public Guid TicketId { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public string ResolutionNote { get; set; } = string.Empty;
        public TicketStatus CurrentStatus { get; set; }
        public DateTime? ClosedAt { get; set; }

    }
}
