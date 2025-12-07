using AOps.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class VehicleDocument : BaseEntity
    {
        public Guid VehicleDocumentId { get; set; }
        public Guid VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public VehicleMaster Vehicle { get; set; }  // Navigation property

        [Required]
        public string DocumentType { get; set; }    // e.g. Insurance, Registration, Emission

        [Required]
        public string FilePath { get; set; }        // Path or URL to the uploaded document

        public DateTime ExpiryDate { get; set; }   // Optional for documents like insurance

        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }

    }
}
