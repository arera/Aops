using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerContracts
{
      public record GetCustomerContractDropdownCommand(Guid CustomerId) : IRequest<List<DTOs.CustomerContracts.CustomerContractDropdown>>;
}
