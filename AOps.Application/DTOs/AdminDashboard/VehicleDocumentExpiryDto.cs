using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AdminDashboard
{
    public class VehicleDocumentExpiryDto
    {
        public string VehicleName { get; set; }
        public string SiteName { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime? PollutionCertificateExpiry { get; set; }
        public string? PollutionCertificateFile { get; set; }

        public DateTime? FitnessCertificateExpiry { get; set; }
        public string? FitnessCertificateFile { get; set; }

        public DateTime? PermitExpiry { get; set; }
        public string? PermitFile { get; set; }

        public DateTime? InsuranceExpiry { get; set; }
        public string? InsuranceFile { get; set; }
    }

}
