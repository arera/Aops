using AOps.Application.DTOs.AdminServiceTickets;
using AOps.Application.DTOs.CustomerTickets;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.CustomerTicket;
using AOps.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminCustomerTicket
{
    public class FetchAllTicketsRaisedHandler : IRequestHandler<FetchAllTicketsRaisedCommand, List<FetchAllTicketsDto>>
    {
        private readonly IAdminCustomerTicket  _repo;
        private readonly ILogger<FetchAllTicketsRaisedHandler> _logger;

        public FetchAllTicketsRaisedHandler(
            IAdminCustomerTicket repo,
            ILogger<FetchAllTicketsRaisedHandler> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<List<FetchAllTicketsDto>> Handle(FetchAllTicketsRaisedCommand request, CancellationToken cancellationToken)
        {
            var TicketMaster = await _repo.GetAllAsync(cancellationToken);

            return TicketMaster.Select(cticket => new FetchAllTicketsDto
            {
                SiteName = cticket.SiteName,
                SubjectName = cticket.SubjectName,
                TicketCategory = cticket.TicketCategory,
                TicketDescription = cticket.TicketDescription,
                TicketNumber = cticket.TicketNumber,
                TicketIssueType = cticket.TicketIssueType,
                TicketPriority = cticket.TicketPriority,
                CurrentStatus = cticket.CurrentStatus,
                CreatedAt = cticket.CreatedAt,
                UpdatedAt = cticket.UpdatedAt,
                ClosedAt = cticket.ClosedAt,
                HandelBy = cticket.HandelBy,
                ResolutionNote = cticket.ResolutionNote,
                TicketId = cticket.TicketId,
            }).ToList();
        }
    }
}
