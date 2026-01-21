using AOps.Application.DTOs.CustomerTickets;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerTicket
{
    public class GetAllCustomerTicketHandler : IRequestHandler<GetAllCustomerTicketCommand,List<GetCustomerTicketDto>>
    {
        private readonly ICustomerTicketRepository _repo;
        private readonly ILogger<GetAllCustomerTicketHandler> _logger;

        public GetAllCustomerTicketHandler(
            ICustomerTicketRepository repo,
            ILogger<GetAllCustomerTicketHandler> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<List<GetCustomerTicketDto>> Handle(GetAllCustomerTicketCommand request, CancellationToken cancellationToken)
        {
            var TicketMaster = await _repo.GetAllAsync(request.CustomerId, cancellationToken);

            return TicketMaster.Select(cticket => new GetCustomerTicketDto
            {
                SiteName = cticket.SiteName,
                SubjectName = cticket.SubjectName,
                TicketCategory = cticket.TicketCategory,
                TicketDescription = cticket.TicketDescription,
                TicketNumber = cticket.TicketNumber,
                TicketIssueType = cticket.TicketIssueType,
                TicketPriority = cticket.TicketPriority,
                TicketSource = cticket.TicketSource,
                CurrentStatus = ((TicketStatus)cticket.TStatus).ToString(),
                CreatedAt = cticket.CreatedAt,
                UpdatedAt = cticket.UpdatedAt
            }).ToList();
        }
    }
}
