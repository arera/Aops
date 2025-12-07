using AOps.Application.Interfaces;
using AOps.Application.UseCases.CustomerSites;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Domain.Entities;
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
    public class CreateTicketHandler:IRequestHandler<CreateTicketCommand, Guid>
    {
        private readonly ICustomerTicketRepository _repo;
        private readonly ILogger<CreateTicketHandler> _logger;
        public CreateTicketHandler(ICustomerTicketRepository repository,  ILogger<CreateTicketHandler> logger)
        {
            _repo = repository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Pick correct subject id based on category
                Guid subjectId = request.obj.TicketCategory switch
                {
                    TicketCategorys.Vehicle => request.obj.VehicleId ?? throw new ArgumentNullException(nameof(request.obj.VehicleId)),
                    TicketCategorys.Manpower => request.obj.EmployeeId ?? throw new ArgumentNullException(nameof(request.obj.EmployeeId)),
                    _ => throw new InvalidOperationException("Unsupported ticket category")
                };
                int issueType = request.obj.TicketCategory switch
                {
                    TicketCategorys.Vehicle => int.TryParse(request.obj.TicketIssueType, out var vehicleIssue) &&
                                               Enum.IsDefined(typeof(VehicleTicketIssueType), vehicleIssue)
                        ? vehicleIssue
                        : throw new ArgumentException($"Invalid Vehicle issue type: {request.obj.TicketIssueType}"),

                    TicketCategorys.Manpower => int.TryParse(request.obj.TicketIssueType, out var manpowerIssue) &&
                                                Enum.IsDefined(typeof(ManpowerTicketIssueType), manpowerIssue)
                        ? manpowerIssue
                        : throw new ArgumentException($"Invalid Manpower issue type: {request.obj.TicketIssueType}"),

                    _ => throw new InvalidOperationException("Unsupported ticket category")
                };


                var ticket = new CustomerTickets
                {
                    TicketId = Guid.NewGuid(),
                    SiteId = request.obj.SiteId,
                    SubjectId = subjectId,
                    TicketCategory = request.obj.TicketCategory,
                    TicketDescription = request.obj.TicketDescription,
                    TicketPriority = request.obj.TicketPriority,
                    TicketStatus = (int)TicketStatus.New,
                    TicketIssueType = issueType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    HandledBy = await _repo.GetSitehandelByAsync(request.obj.SiteId, cancellationToken)
                };

                // Save ticket
                var result = await _repo.AddAsync(ticket, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer ticket for SiteId: {SiteId}", request.obj.SiteId);
                throw;
            }
        }

    }
}
