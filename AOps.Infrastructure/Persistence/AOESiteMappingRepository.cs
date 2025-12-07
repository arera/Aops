using AOps.Application.DTOs.AOESite;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class AOESiteMappingRepository : IAOESiteMappingRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<AOESiteMappingRepository> _logger;
        private readonly IHostEnvironment _env;

        public AOESiteMappingRepository(
            AOpsDbContext context,
            IHostEnvironment env,
            ILogger<AOESiteMappingRepository> logger)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        public async Task<Guid> AddAsync(AOESiteMapping emp, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.AOESiteMapping.AddAsync(emp, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("AOE site mapping added with MappingId: {MappingId}", emp.MappingId);

                return emp.MappingId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding AOE site mapping");
                throw;
            }
        }

        public async Task<List<GetAoeSiteDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var data = await _context.AOESiteMapping
                    .AsNoTracking()
                    .Include(m => m.CustomerSite)
                    .Include(m => m.Orglevels)
                    .ToListAsync(cancellationToken);

                var result = data
                    .GroupBy(m => m.OrgEmployeeId)
                    .Select(g => new GetAoeSiteDto
                    {
                        MappingId = g.First().MappingId,
                        AOEName = g.First().Orglevels.Name,      
                        SiteNames = g.Select(x => x.CustomerSite.SiteName).ToList()
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all AOE site mappings");
                throw;
            }
        }


        public async Task<AOESiteMapping> GetAoeBySiteIdAsync(Guid aoeId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.AOESiteMapping
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.OrgEmployeeId == aoeId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving AOE site mapping by AOE ID: {AoeId}", aoeId);
                throw;
            }
        }

        public async Task<int> UpdateAoeSiteAsync(AOESiteMapping aoeSite, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.AOESiteMapping.Update(aoeSite);
                var result = await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("AOE site mapping updated with MappingId: {MappingId}", aoeSite.MappingId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating AOE site mapping");
                throw;
            }
        }

        public async Task AddRangeAsync(IEnumerable<AOESiteMapping> mappings, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.AOESiteMapping.AddRangeAsync(mappings, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                // Handle specific SQL Server errors (2627 = PK violation, 547 = FK violation, 2601 = unique constraint)
                if (sqlEx.Number == 547) // Foreign Key violation
                {
                    throw new InvalidOperationException(
                        "One or more SiteIds are invalid. Please ensure all SiteIds exist in CustomerSite.",
                        ex
                    );
                }
                else if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Duplicate key
                {
                    throw new InvalidOperationException(
                        "Duplicate mapping detected. The same SiteId may already be mapped.",
                        ex
                    );
                }

                // If it's another SQL error, rethrow
                throw;
            }
            catch (Exception ex)
            {
                // Catch-all for other unexpected errors
                throw new ApplicationException("An unexpected error occurred while saving AOESiteMappings.", ex);
            }
        }


    }

}
