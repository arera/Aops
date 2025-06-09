using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs
{
    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new AddressDto();
        public string PrimaryMobile { get; set; } = string.Empty;
        public string? SecondaryMobile { get; set; }
        public string GST { get; set; } = string.Empty;
    }

    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

}
