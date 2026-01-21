using AOps.Application.DTOs.Employees;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.EmployeeMasters
{
    public record FetchEmployeeByIdCommand(Guid EmployeeId) : IRequest<FetchEmployeesDto>;
}
