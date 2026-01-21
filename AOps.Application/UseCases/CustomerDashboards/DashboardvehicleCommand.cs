using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerDashboards
{
    public record DashboardvehicleCommand(Guid CustomerId) : IRequest<List<DTOs.CustomerDashboard.VehiclesDto>>;
}
