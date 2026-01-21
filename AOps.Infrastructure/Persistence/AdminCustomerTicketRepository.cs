using AOps.Application.DTOs.AdminServiceTickets;
using AOps.Application.DTOs.CustomerTickets;
using AOps.Application.Interfaces;
using AOps.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class AdminCustomerTicketRepository : IAdminCustomerTicket
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<AdminCustomerTicketRepository> _logger;
        public AdminCustomerTicketRepository(AOpsDbContext context, ILogger<AdminCustomerTicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<FetchAllTicketsDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var query =
                    from ticket in _context.CustomerTickets.AsNoTracking()
                    join site in _context.CustomerSite.AsNoTracking()
                        on ticket.SiteId equals site.SiteId
                    join contract in _context.CustomerContract.AsNoTracking()
                        on site.ContractId equals contract.ContractId
                    join customer in _context.Customers.AsNoTracking()
                        on contract.CustomerId equals customer.UserId
                    join handler in _context.OrganisationLevels.AsNoTracking()
                      on ticket.HandledBy equals handler.UserID into handlerGroup
                    from handler in handlerGroup.DefaultIfEmpty()
                    orderby ticket.CreatedAt descending
                    select new
                    {
                        Ticket = ticket,
                        Site = site,
                        HandledByName = handler != null ? handler.Name : "NA",
                        
                    };

                var results = await query.ToListAsync(cancellationToken);

                var ticketDtos = results.Select(x =>
                {
                    string subjectName = string.Empty;
                    string issueType = string.Empty;

                    if (x.Ticket.TicketCategory == TicketCategorys.Vehicle)
                    {
                        var vehicle = _context.VehicleMaster
                            .AsNoTracking()
                            .FirstOrDefault(v => v.VehicleId == x.Ticket.SubjectId);

                        subjectName = vehicle?.VehicleNumber ?? "Unknown Vehicle";

                        issueType = Enum.IsDefined(typeof(VehicleTicketIssueType), x.Ticket.TicketIssueType)
                            ? ((VehicleTicketIssueType)x.Ticket.TicketIssueType).ToString()
                            : "Invalid Vehicle IssueType";
                    }
                    else if (x.Ticket.TicketCategory == TicketCategorys.Manpower)
                    {
                        var employee = _context.Employeemaster
                            .AsNoTracking()
                            .FirstOrDefault(e => e.EmployeeId == x.Ticket.SubjectId);

                        subjectName = employee?.Name ?? "Unknown Employee";

                        issueType = Enum.IsDefined(typeof(ManpowerTicketIssueType), x.Ticket.TicketIssueType)
                            ? ((ManpowerTicketIssueType)x.Ticket.TicketIssueType).ToString()
                            : "Invalid Manpower IssueType";
                    }

                    return new FetchAllTicketsDto
                    {
                        TicketId = x.Ticket.TicketId,
                        SiteName = x.Site.SiteName,
                        TicketPriority = x.Ticket.TicketPriority,
                        TicketDescription = x.Ticket.TicketDescription,
                        TicketCategory = x.Ticket.TicketCategory,
                        CurrentStatus = x.Ticket.TicketStatus,
                        TicketIssueType = issueType,
                        CreatedAt = x.Ticket.CreatedAt,
                        UpdatedAt = x.Ticket.UpdatedAt,
                        TicketNumber = x.Ticket.Id,
                        SubjectName = subjectName,
                        HandelBy = x.HandledByName,
                        ClosedAt = x.Ticket.ClosedAt,
                        ResolutionNote = x.Ticket.ResolutionNote
                        
                        
                    };
                }).ToList();

                return ticketDtos;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("GetAllAsync was canceled");
                return Enumerable.Empty<FetchAllTicketsDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tickets");
                return Enumerable.Empty<FetchAllTicketsDto>();
            }

        }
        public async Task<int> UpdateTicketAsync(UpdateServiceTicketsDto ticketDto, CancellationToken cancellationToken = default)
        {
            var ticket = await _context.CustomerTickets
                .FirstOrDefaultAsync(t => t.TicketId == ticketDto.TicketId, cancellationToken);

            if (ticket == null)
                return 0; // or throw new KeyNotFoundException("Ticket not found");

            // ✅ Update fields
            ticket.TicketPriority = ticketDto.TicketPriority;
            ticket.ResolutionNote = ticketDto.ResolutionNote;
            ticket.TicketStatus = ticketDto.CurrentStatus;
            ticket.UpdatedAt = DateTime.UtcNow;
            

            // If ticket is closed, update ClosedAt
            if (ticketDto.CurrentStatus == TicketStatus.Closed)
            {
                ticket.ClosedAt = ticketDto.ClosedAt != default
                    ? ticketDto.ClosedAt
                    : DateTime.UtcNow;
            }

            _context.CustomerTickets.Update(ticket);

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<FetchAllTicketsDto?> GetByIdAsync(Guid TicketId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result =
                    await (from ticket in _context.CustomerTickets.AsNoTracking()
                           join site in _context.CustomerSite.AsNoTracking()
                               on ticket.SiteId equals site.SiteId
                           join contract in _context.CustomerContract.AsNoTracking()
                               on site.ContractId equals contract.ContractId
                           join customer in _context.Customers.AsNoTracking()
                               on contract.CustomerId equals customer.UserId
                           join handler in _context.OrganisationLevels.AsNoTracking()
                               on ticket.HandledBy equals handler.UserID into handlerGroup
                           from handler in handlerGroup.DefaultIfEmpty()
                           where ticket.TicketId == TicketId
                           select new
                           {
                               Ticket = ticket,
                               Site = site,
                               HandledByName = handler != null ? handler.Name : "NA"
                           }).FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                    return null;

                string subjectName = string.Empty;
                string issueType = string.Empty;

                if (result.Ticket.TicketCategory == TicketCategorys.Vehicle)
                {
                    var vehicle = await _context.VehicleMaster
                        .AsNoTracking()
                        .FirstOrDefaultAsync(v => v.VehicleId == result.Ticket.SubjectId, cancellationToken);

                    subjectName = vehicle?.VehicleNumber ?? "Unknown Vehicle";

                    issueType = Enum.IsDefined(typeof(VehicleTicketIssueType), result.Ticket.TicketIssueType)
                        ? ((VehicleTicketIssueType)result.Ticket.TicketIssueType).ToString()
                        : "Invalid Vehicle IssueType";
                }
                else if (result.Ticket.TicketCategory == TicketCategorys.Manpower)
                {
                    var employee = await _context.Employeemaster
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.EmployeeId == result.Ticket.SubjectId, cancellationToken);

                    subjectName = employee?.Name ?? "Unknown Employee";

                    issueType = Enum.IsDefined(typeof(ManpowerTicketIssueType), result.Ticket.TicketIssueType)
                        ? ((ManpowerTicketIssueType)result.Ticket.TicketIssueType).ToString()
                        : "Invalid Manpower IssueType";
                }

                return new FetchAllTicketsDto
                {
                    TicketId = result.Ticket.TicketId,
                    SiteName = result.Site.SiteName,
                    TicketPriority = result.Ticket.TicketPriority,
                    TicketDescription = result.Ticket.TicketDescription,
                    TicketCategory = result.Ticket.TicketCategory,
                    TicketIssueType = issueType,
                    CreatedAt = result.Ticket.CreatedAt,
                    UpdatedAt = result.Ticket.UpdatedAt,
                    TicketNumber = result.Ticket.Id,
                    SubjectName = subjectName,
                    HandelBy = result.HandledByName,
                    ClosedAt = result.Ticket.ClosedAt,
                    CurrentStatus = result.Ticket.TicketStatus,
                    ResolutionNote = result.Ticket.ResolutionNote
                };
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("GetByIdAsync was canceled");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ticket");
                return null;
            }
        }

        
    }
}
