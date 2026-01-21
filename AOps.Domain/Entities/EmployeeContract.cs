using AOps.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class EmployeeContract : BaseEntity
    {
        public string ContractId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string Description { get; set; }
        public string ContractDocument {  get; set; }
       
    }
}
