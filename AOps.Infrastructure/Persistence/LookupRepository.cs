using AOps.Application.DTOs.CustomerSites;
using AOps.Application.DTOs.DropDown;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class LookupRepository : ILookupRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<LookupRepository> _logger;
        public LookupRepository(AOpsDbContext context, ILogger<LookupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<SiteSelectDto>> GetSiteByCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var result = await (
                from site in _context.CustomerSite
                join contract in _context.CustomerContract
                    on site.ContractId equals contract.ContractId
                join customer in _context.Customers
                    on contract.CustomerId equals customer.UserId
                where customer.UserId == customerId
                select new SiteSelectDto
                {
                    SiteId = site.SiteId,
                    SiteName = site.SiteName
                }
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<List<SiteSelectDto>> GetSiteByCustomerAsync(CancellationToken cancellationToken)
        {
            var result = await (
                from site in _context.CustomerSite
                join contract in _context.CustomerContract
                    on site.ContractId equals contract.ContractId
                join customer in _context.Customers
                    on contract.CustomerId equals customer.UserId
                select new SiteSelectDto
                {
                    SiteId = site.SiteId,
                    SiteName = site.SiteName
                }
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<List<VehicleSelectDto>> GetVehcleBySiteAsync(Guid siteId, CancellationToken cancellationToken)
        {
            var result = await (
                from sva in _context.SiteVehicleAssignment
                join vehicle in _context.VehicleMaster
                    on sva.VehicleId equals vehicle.VehicleId
                where sva.SiteId == siteId
                select new VehicleSelectDto
                {
                    VehicleId = vehicle.VehicleId,
                    VehicleName = vehicle.VehicleNumber
                }
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            return result; 
        }
        public async Task<List<UsersByRoleDto>> GetUsersByRoleAsync(int roleId, CancellationToken cancellationToken)
        {
            return await (from user in _context.OrganisationLevels
                          where user.Role == roleId
                          select new UsersByRoleDto
                          {
                              UserId = user.UserID,
                              UserName = user.Name,
                          })
                          .ToListAsync(cancellationToken);
        }
        public async Task<List<EmployeeSelectDto>> GetAllEmployeeAsync( CancellationToken cancellationToken)
        {
            return await (from emp in _context.Employeemaster
                         
                          select new EmployeeSelectDto
                          {
                              EmployeeId = emp.EmployeeId,
                              EmployeeName = emp.Name,
                          })
                          .ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeeSelectDto>> GetEmployeeBySiteAsync(Guid siteId, CancellationToken cancellationToken)
        {
            var result = await (
                from sva in _context.SiteEmployeeAssignment
                join Emp in _context.Employeemaster
                    on sva.EmployeeId equals Emp.EmployeeId
                where sva.SiteId == siteId
                select new EmployeeSelectDto
                {
                    EmployeeId = Emp.EmployeeId,
                    EmployeeName = Emp.Name,
                }
                         
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            return result;
        }
    }
}
