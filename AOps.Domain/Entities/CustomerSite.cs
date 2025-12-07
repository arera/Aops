using AOps.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class CustomerSite : BaseEntity
    {
        public Guid SiteId { get; set; }
        public string ContractId {  get; set; }
        public string SiteName { get;set;}
        public string SiteCity { get;set;}
        public string SiteState { get; set;}
        public string SiteZip { get;set;}
        public string SiteCountry { get;set;}
        public ICollection<SiteVehicleAssignment> SiteVehicleAssignments { get; set; } = new List<SiteVehicleAssignment>();
        public ICollection<SiteEmployeeAssignment> SiteEmployeeAssignments { get; set; } = new List<SiteEmployeeAssignment>();
    }
}
