using AOps.Application.DTOs.EmpContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.EmployeeContract
{ 
    public record FetchEmployeeContractCommand() : IRequest<List<FetchAllEmployeeContractDto>>;
}
