using AOps.Application.DTOs.CustomerDashboard;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using AOps.Domain.Enums;
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
    public class CustomerDashboardRepository : ICustomerDashboardsRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CustomerDashboardRepository> _logger;

        public CustomerDashboardRepository(AOpsDbContext context, ILogger<CustomerDashboardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<VehiclesDto>> GetAllVehicleAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var vehicles =
    from cc in _context.CustomerContract.AsNoTracking()
    join cs in _context.CustomerSite.AsNoTracking() on cc.ContractId equals cs.ContractId
    join sva in _context.SiteVehicleAssignment.AsNoTracking() on cs.SiteId equals sva.SiteId
    join vm in _context.VehicleMaster.AsNoTracking() on sva.VehicleId equals vm.VehicleId
    join vd in _context.VehicleDocument.AsNoTracking() on vm.VehicleId equals vd.VehicleId
    where cc.CustomerId == customerId
    group vd by new
    {
        vm.VehicleId,
        vm.VehicleNumber,
        cs.SiteName
    } into g
    select new VehiclesDto
    {
        VehicleName = g.Key.VehicleNumber,
        SiteName = g.Key.SiteName,
        PollutionCertificate = g
            .Where(d => d.DocumentType == "PollutionCertificate")
            .Select(d => d.ExpiryDate)
            .Max(),
        FitnessCertificate = g
            .Where(d => d.DocumentType == "FitnessCertificate")
            .Select(d => d.ExpiryDate)
            .Max(),
        Permit = g
            .Where(d => d.DocumentType == "Permit")
            .Select(d => d.ExpiryDate)
            .Max(),
        Insurance = g
            .Where(d => d.DocumentType == "Insurance")
            .Select(d => d.ExpiryDate)
            .Max()
        //Registration = g
        //    .Where(d => d.DocumentType == "Registration")
        //    .Select(d => d.ExpiryDate)
        //    .Max()
    };


            return await vehicles.ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeesDto>> GetAllEmployeeAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var result = from e in _context.Employeemaster.AsNoTracking()
                         join sa in _context.SiteEmployeeAssignment.AsNoTracking()
                             on e.EmployeeId equals sa.EmployeeId
                         join cs in _context.CustomerSite.AsNoTracking()
                             on sa.SiteId equals cs.SiteId
                         join cc in _context.CustomerContract.AsNoTracking()
                             on cs.ContractId equals cc.ContractId
                         where cc.CustomerId == customerId // filter by customer
                         select new EmployeesDto
                         {
                             EmpName = e.Name,
                             Designation = ((Designation)e.DesignationId).ToString(), // enum cast
                             Gender = e.Gender,
                             SiteName = cs.SiteName
                         };

            return await result.ToListAsync(cancellationToken);
        }

    }
}
