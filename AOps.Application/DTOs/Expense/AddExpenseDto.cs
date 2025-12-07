using AOps.Application.DTOs.DropDown;
using AOps.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Expense
{
    public class AddExpenseDto
    {
        public Guid SiteId { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public float ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseRemark { get; set; }
        public Guid ExecutiveId { get; set; }
        public IFormFile? ReceiptDocumentFile { get; set; }
        public string ReceiptPath { get; set;}
        public List<SiteSelectDto> SiteList { get; set; } = new();
        public List<UsersByRoleDto> ExecutiveList { get; set; } = new();
    }
}
