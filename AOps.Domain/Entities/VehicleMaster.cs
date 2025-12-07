using AOps.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class VehicleMaster : BaseEntity
    {
        public Guid VehicleId { get; set; }

        [ForeignKey("VendorId")]
        public VendorMaster Vendor { get; set; } = null!;
        public Guid VendorId { get; set; }

        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleFuelType { get; set; }
        public string AmbulanceType { get; set; }
        public string RegistrationDocument { get; set;}

        public string VendorContractId { get; set; }

        public ICollection<VehicleDocument> Documents { get; set; } = new List<VehicleDocument>();
    }
}
