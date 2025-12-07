using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.Vendor
{
    public record VendorDropDownCommand: IRequest<List<DTOs.Vendor.VendorDropdownDto>>;
    
}
