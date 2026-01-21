using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.CustomerDashboard
{
    public class VehiclesDto
    {
        public string VehicleName {get;set;}
        public string SiteName { get; set;}
        public DateTime PollutionCertificate { get; set; }
        public DateTime FitnessCertificate { get; set; }
        public DateTime Permit {  get; set; }
        public DateTime Insurance { get; set; }
    }
}
