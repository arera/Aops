using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Expense
{
    public class FetchExpenseDto
    {
        public Guid ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public string SiteName { get; set; }
        public string ExecutiveName { get; set;}
        public float ExpenseAmount { get; set; }
        public string ExpenseRemark { get; set;}
        public string ExpenseReceipt { get; set;}
        public DateTime ExpenseDate { get; set; }
        public DateTime Created_on { get; set; }
    }
}
