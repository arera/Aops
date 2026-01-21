using AOps.Application.DTOs.AdminServiceTickets;
using AOps.Application.Interfaces;
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
    public class FetchTicketByIdHandler : IRequestHandler<FetchTicketByIdCommand, FetchAllTicketsDto>
    {
        private readonly IAdminCustomerTicket _repo;
        private readonly ILogger<FetchTicketByIdHandler> _logger;

    public FetchTicketByIdHandler(
        IAdminCustomerTicket repo,
        ILogger<FetchTicketByIdHandler> logger,
        string siteUrl)
    {
        _repo = repo;
        _logger = logger;
    }
        public async Task<FetchAllTicketsDto> Handle(FetchTicketByIdCommand request, CancellationToken cancellationToken)
        {
            var cticket = await _repo.GetByIdAsync(request.Ticketid, cancellationToken);

            if (cticket == null) return null;

            return new FetchAllTicketsDto
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
                ResolutionNote = cticket.ResolutionNote
            };
        }
    }

}
