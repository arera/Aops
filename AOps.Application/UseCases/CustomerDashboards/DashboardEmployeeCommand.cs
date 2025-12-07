using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerDashboards
{
    public record DashboardEmployeeCommand(Guid CustomerId) : IRequest<List<DTOs.CustomerDashboard.EmployeesDto>>;
}

