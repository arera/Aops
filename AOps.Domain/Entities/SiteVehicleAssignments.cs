using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class SiteVehicleAssignment
    {
        public int Id { get; set;}
        public Guid SiteId { get; set; }
        public CustomerSite CustomerSite { get; set; }
        public Guid VehicleId { get; set; }
        public VehicleMaster Vehicle { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool IsAssigned { get; set; }
        public Guid AssignedBy { get; set; }

    }
}
