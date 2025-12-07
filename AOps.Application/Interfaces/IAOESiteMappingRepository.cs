using AOps.Application.DTOs.AOESite;
using AOps.Application.DTOs.CustomerSites;
using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface IAOESiteMappingRepository
    {
        Task<Guid> AddAsync(AOESiteMapping Emp, CancellationToken cancellationToken = default);

        Task<List<GetAoeSiteDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<AOESiteMapping> GetAoeBySiteIdAsync(Guid AoeId, CancellationToken cancellationToken=default);

        Task<int> UpdateAoeSiteAsync(AOESiteMapping aoesite, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<AOESiteMapping> mappings, CancellationToken cancellationToken = default);

    }
}
