using AOps.Application.DTOs.AdminDashboard;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminDashboards
{
    public record ExpiredVehicleDocumentsCommand() : IRequest<List<VehicleDocumentExpiryDto>>;
}
