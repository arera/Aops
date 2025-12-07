using AOps.Application.DTOs.CustomerTickets;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
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
    public class CustomerTicketRepository : ICustomerTicketRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CustomerTicketRepository> _logger;
        public CustomerTicketRepository(AOpsDbContext context, ILogger<CustomerTicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(CustomerTickets ctickets, CancellationToken cancellationToken = default)
        {
         
            try
            {
                await _context.CustomerTickets.AddAsync(ctickets, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return ctickets.TicketId;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database Insertion error while creating ticket with Customer: {Customer}", ctickets.TicketId);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating ticket with Customer: {Customer}", ctickets.TicketId);
                throw;
            }
        }
        public async Task<IEnumerable<GetCustomerTicketDto>> GetAllAsync(Guid customerId, CancellationToken cancellationToken = default)
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
                    where customer.UserId == customerId
                    orderby ticket.CreatedAt descending
                    select new
                    {
                        Ticket = ticket,
                        Site = site
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

                    return new GetCustomerTicketDto
                    {
                        TicketId = x.Ticket.TicketId,
                        SiteName = x.Site.SiteName,
                        TicketPriority = x.Ticket.TicketPriority,
                        TicketDescription = x.Ticket.TicketDescription,
                        TicketCategory = x.Ticket.TicketCategory,
                        TicketIssueType = issueType,
                        CreatedAt = x.Ticket.CreatedAt,
                        UpdatedAt = x.Ticket.UpdatedAt,
                        TicketNumber = x.Ticket.Id,
                        SubjectName = subjectName
                    };
                }).ToList();

                return ticketDtos;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("GetAllAsync was canceled for CustomerId: {CustomerId}", customerId);
                return Enumerable.Empty<GetCustomerTicketDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tickets for CustomerId: {CustomerId}", customerId);
                return Enumerable.Empty<GetCustomerTicketDto>();
            }

        }



        public Task<int> UpdateVehicleAsync(CustomerTickets Tickets, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> GetSitehandelByAsync(Guid siteId, CancellationToken cancellationToken = default)
        {
            return await _context.AOESiteMapping
                .Where(x => x.SiteId == siteId)
                .Select(x => x.OrgEmployeeId).FirstOrDefaultAsync(cancellationToken);
        }

    }
}
