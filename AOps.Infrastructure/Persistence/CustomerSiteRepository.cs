using AOps.Application.DTOs.CustomerSites;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AOps.Infrastructure.Persistence
{
    public class CustomerSiteRepository : ICustomerSiteRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CustomerSiteRepository> _logger;
        public CustomerSiteRepository(AOpsDbContext context, ILogger<CustomerSiteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> AddAsync(CustomerSite ccontract, CancellationToken cancellationToken = default)
        {
            await _context.CustomerSite.AddAsync(ccontract, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<GetCustomerSiteDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result =    from site in _context.CustomerSite
             join contract in _context.CustomerContract
            on site.ContractId equals contract.ContractId
              join customer in _context.Customers
          on contract.CustomerId equals customer.UserId
           
    select new GetCustomerSiteDto
    {
        SiteId = site.SiteId,
        SiteName = site.SiteName,
        SiteCity = site.SiteCity,
        SiteState = site.SiteState,
        SiteZip = site.SiteZip,
        SiteCountry = site.SiteCountry,
        CustomerName = customer.Name,
        CustomerId = customer.UserId,
        CustomerContract = contract.ContractId.ToString(), // or contract.ContractName
        Vehicle_number_list = (
            from sva in _context.SiteVehicleAssignment
            where sva.SiteId == site.SiteId
            join vehicle in _context.VehicleMaster
                on sva.VehicleId equals vehicle.VehicleId
            select vehicle.VehicleNumber
        ).ToList(),
        Employee_name_list = (
            from sva in _context.SiteEmployeeAssignment
            where sva.SiteId == site.SiteId
            join EmployeeMaster in _context.Employeemaster
                on sva.EmployeeId equals EmployeeMaster.EmployeeId
            select EmployeeMaster.Name
        ).ToList(),
        AssignedVehicleIds = (
                from sva in _context.SiteVehicleAssignment
                where sva.SiteId == site.SiteId
                select sva.VehicleId
            ).ToList(),

             AssignedEmployeeIds = (
                from sva in _context.SiteEmployeeAssignment
                where sva.SiteId == site.SiteId
                select sva.EmployeeId
            ).ToList()
    };


            return await result
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<GetCustomerSiteDto> GetSiteByIdAsync(Guid siteId, CancellationToken cancellationToken = default)
        {
            var result = await (
                from site in _context.CustomerSite
                join contract in _context.CustomerContract
                    on site.ContractId equals contract.ContractId
                join customer in _context.Customers
                    on contract.CustomerId equals customer.UserId
                where site.SiteId == siteId
                select new GetCustomerSiteDto
                {
                    SiteId = site.SiteId,
                    SiteName = site.SiteName,
                    SiteCity = site.SiteCity,
                    SiteState = site.SiteState,
                    SiteZip = site.SiteZip,
                    SiteCountry = site.SiteCountry,
                    CustomerName = customer.Name,
                    CustomerContract = contract.ContractId.ToString(), // or contract.ContractName
                    Vehicle_number_list = (
                        from sva in _context.SiteVehicleAssignment
                        where sva.SiteId == site.SiteId
                        join vehicle in _context.VehicleMaster
                            on sva.VehicleId equals vehicle.VehicleId
                        select vehicle.VehicleNumber
                    ).ToList(),
                    AssignedVehicleIds = (
                from sva in _context.SiteVehicleAssignment
                where sva.SiteId == site.SiteId
                select sva.VehicleId
            ).ToList()
                }
            )
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken); // ✅ only one record

            return result;
        }


        public async Task<CustomerSite?> GetByIdWithVehiclesAsync(Guid siteId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.CustomerSite
                    .Include(s => s.SiteVehicleAssignments)
                    .Include(s => s.SiteEmployeeAssignments)
                    .FirstOrDefaultAsync(s => s.SiteId == siteId, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("GetByIdWithVehiclesAsync was canceled for SiteId: {SiteId}", siteId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching site with vehicles for SiteId: {SiteId}", siteId);
                return null;
            }
        }




        public async Task<int> UpdateSiteAsync(CustomerSite ccontract, CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }



    }
}
