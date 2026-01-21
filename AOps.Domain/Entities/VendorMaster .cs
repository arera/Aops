using AOps.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace AOps.Domain.Entities
{
    [Table("VendorMaster")]
    public class VendorMaster : BaseEntity
    {
        public string VendorName { get; set; } = string.Empty;
        public Guid VendorId { get; set;} = Guid.Empty;
        public string VendorMobile { get; set; } = string.Empty;
        public string VendorEmail { get; set; } = string.Empty;
        public string VendorAddress { get; set; } = string.Empty;

        public ICollection<VendorContract> Contracts { get; set; } = new List<VendorContract>();
    }
}
