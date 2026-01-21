using AOps.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class AOESiteMapping : BaseEntity
    {
        public Guid MappingId {get; set;}
        public Guid OrgEmployeeId {get; set;}
        public Guid SiteId {get; set;}

        [ForeignKey(nameof(SiteId))]
        public virtual CustomerSite CustomerSite {get; set;}
        [ForeignKey(nameof(OrgEmployeeId))]
        public virtual Orglevels Orglevels {get; set;}
    }
}
