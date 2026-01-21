using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.VendorContracts
{
    public record AddVendorContractCommand(DTOs.Vendor.AddCustomerContractDto obj):IRequest<int>;
    
}
