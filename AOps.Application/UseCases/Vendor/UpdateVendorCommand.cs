using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.Vendor
{
    public record UpdateVendorCommand(Guid VendorId, string VendorName, string VendorMobile, string VendorEmail, string VendorAddress,bool IsActive) :IRequest<int>;
   
}
