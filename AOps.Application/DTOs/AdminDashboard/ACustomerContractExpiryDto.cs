using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AdminDashboard
{
    public class ACustomerContractExpiryDto
    {
        public string CustomerName { get; set;}
        public string ContractId { get; set;}
        public DateTime ExpireOn { get; set;}
        public string ContractDocument {  get; set;}
    }
}
