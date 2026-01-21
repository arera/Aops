using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.VehicleDocuments
{
    public class FetchVehicleDocumentsDto
    {
        public string VehicleNumber { get; set; } = string.Empty;
        public string DocumentName { get; set;} = string.Empty;
        public DateTime ExpireOn { get; set; }
        public string DocumentPath { get; set;} = string.Empty;
        public Guid DocumentId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid VehicleId { get; set; }
    }
}
