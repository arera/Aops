using AOps.Application.DTOs.AdminDashboard;
using AOps.Application.DTOs.CustomerDashboard;
using AOps.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<AdminDashboardRepository> _logger;

        public AdminDashboardRepository(AOpsDbContext context, ILogger<AdminDashboardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<VehicleDocumentExpiryDto>> GetAllExpiredVehicleDocumentsAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            var next30Days = today.AddDays(30);

            // Step 1: Query all data in one database call
            var rawData = await (
                from cc in _context.CustomerContract.AsNoTracking()
                join cs in _context.CustomerSite.AsNoTracking() on cc.ContractId equals cs.ContractId
                join sva in _context.SiteVehicleAssignment.AsNoTracking() on cs.SiteId equals sva.SiteId
                join vm in _context.VehicleMaster.AsNoTracking() on sva.VehicleId equals vm.VehicleId
                join vd in _context.VehicleDocument.AsNoTracking() on vm.VehicleId equals vd.VehicleId
                select new
                {
                    vm.VehicleId,
                    vm.VehicleNumber,
                    cs.SiteName,
                    vd.DocumentType,
                    vd.ExpiryDate,
                    vd.FilePath
                }
            ).ToListAsync(cancellationToken);

            // Step 2: Group in-memory
            var grouped = rawData
                .GroupBy(v => new { v.VehicleId, v.VehicleNumber, v.SiteName })
                .Select(g =>
                {
                    var pollution = g.Where(d => d.DocumentType == "PollutionCertificate")
                                     .OrderByDescending(d => d.ExpiryDate)
                                     .FirstOrDefault();

                    var fitness = g.Where(d => d.DocumentType == "FitnessCertificate")
                                   .OrderByDescending(d => d.ExpiryDate)
                                   .FirstOrDefault();

                    var permit = g.Where(d => d.DocumentType == "Permit")
                                  .OrderByDescending(d => d.ExpiryDate)
                                  .FirstOrDefault();

                    var insurance = g.Where(d => d.DocumentType == "Insurance")
                                     .OrderByDescending(d => d.ExpiryDate)
                                     .FirstOrDefault();

                    // 🔥 No null-forgiving, assuming all exist
                    return new VehicleDocumentExpiryDto
                    {
                        VehicleId = g.Key.VehicleId,
                        VehicleName = g.Key.VehicleNumber,
                        SiteName = g.Key.SiteName,
                        PollutionCertificateExpiry = pollution?.ExpiryDate,
                        PollutionCertificateFile = pollution?.FilePath,
                        FitnessCertificateExpiry = fitness?.ExpiryDate,
                        FitnessCertificateFile = fitness?.FilePath,
                        PermitExpiry = permit?.ExpiryDate,
                        PermitFile = permit?.FilePath,
                        InsuranceExpiry = insurance?.ExpiryDate,
                        InsuranceFile = insurance?.FilePath
                    };
                })
                // Step 3: Filter vehicles whose any document expires within 30 days
                .Where(x =>
                    (x.PollutionCertificateExpiry >= today && x.PollutionCertificateExpiry <= next30Days) ||
                    (x.FitnessCertificateExpiry >= today && x.FitnessCertificateExpiry <= next30Days) ||
                    (x.PermitExpiry >= today && x.PermitExpiry <= next30Days) ||
                    (x.InsuranceExpiry >= today && x.InsuranceExpiry <= next30Days)
                )
                .ToList();

            return grouped;
        }



        public async Task<List<AVendorContractExpiryDto>> GetAllVendorContractAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            var next30Days = today.AddDays(30);

            var query =
                from vm in _context.VendorMaster.AsNoTracking()
                join vc in _context.VendorContract.AsNoTracking()
                    on vm.VendorId equals vc.VendorId
                where vc.EndDate >= today && vc.EndDate <= next30Days
                select new AVendorContractExpiryDto
                {
                    VendorName = vm.VendorName,
                    ContractId = vc.ContractId,
                    ExpireOn = vc.EndDate,
                    ContractDocument = vc.AgreementDocument
                };

            return await query.ToListAsync(cancellationToken);
        }


        public async Task<List<ACustomerContractExpiryDto>> GetAllCustomerContractAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            var next30Days = today.AddDays(30);

            var query =
                from vm in _context.Customers.AsNoTracking()
                join vc in _context.CustomerContract.AsNoTracking()
                    on vm.UserId equals vc.CustomerId
                where vc.EndDate >= today && vc.EndDate <= next30Days
                select new ACustomerContractExpiryDto
                {
                    CustomerName = vm.Name,
                    ContractId = vc.ContractId,
                    ExpireOn = vc.EndDate,
                    ContractDocument = vc.AgreementDocument
                };

            return await query.ToListAsync(cancellationToken);
        }

    }
}
