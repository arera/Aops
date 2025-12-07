using AOps.Application.DTOs.CustomerDashboard;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerDashboards
{
   public class DashboardEmployeeHandler : IRequestHandler<DashboardEmployeeCommand, List<EmployeesDto>>
    {
        private readonly ICustomerDashboardsRepository _repo;
        private readonly ILogger<DashboardEmployeeCommand> _logger;
        private readonly string _siteUrl;

        public DashboardEmployeeHandler(
            ICustomerDashboardsRepository repo,
            ILogger<DashboardEmployeeCommand> logger,
            string siteUrl)
        {
            _repo = repo;
            _logger = logger;
            _siteUrl = siteUrl;
        }

        public async Task<List<EmployeesDto>> Handle(DashboardEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Fetch vehicles from repository
            var employeeList = await _repo.GetAllEmployeeAsync(request.CustomerId, cancellationToken);

            // Map to VehiclesDto
            var empdto = employeeList.Select(emaster => new EmployeesDto
            {
                EmpName = emaster.EmpName,
                Designation = emaster.Designation,
                Gender = emaster.Gender,
                SiteName = emaster.SiteName,
            }).ToList();

            return empdto;
        }

    }
}
