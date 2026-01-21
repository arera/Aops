using AOps.Domain.Entities.Common;
using AOps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class SiteExpenses : BaseEntity
    {
        public Guid ExpenseId { get; set; }
        public Guid SiteId { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public DateTime ExpenseDate { get; set; }
        public float ExpenseAmount { get; set; }
        public string ExpenseRemarks { get; set; }
        public Guid ExecutiveId { get; set; }

        public string ExpenseReceipt { get; set; }
    }
}
