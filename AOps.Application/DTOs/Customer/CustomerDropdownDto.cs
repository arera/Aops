using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.Customer
{
    public class CustomerDropdownDto
    {
        public Guid CustomerId { get;set;}
        public string CustomerName { get;set;}= string.Empty;
    }
}
