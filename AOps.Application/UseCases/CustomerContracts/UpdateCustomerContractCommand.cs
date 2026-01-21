using AOps.Application.DTOs.CustomerContracts;
using AOps.Application.DTOs.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerContracts
{
    public record UpdateCustomerContractCommand(EditCustomerContractDto obj):IRequest<int>;
    
}
