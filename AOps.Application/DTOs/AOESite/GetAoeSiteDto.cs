using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AOESite
{
    public class GetAoeSiteDto
    {
        public Guid MappingId { get; set; }
        public string AOEName { get; set; }
        public List<string> SiteNames { get; set; }
        
    }
}
