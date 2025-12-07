using AOps.Application.DTOs.DropDown;
using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LookUp
{
    
    public class EmployeeBySiteHandler : IRequestHandler<EmployeeBySiteCommand, List<EmployeeSelectDto>>
    {
        private ILookupRepository _repo;

        public EmployeeBySiteHandler(ILookupRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<EmployeeSelectDto>> Handle(EmployeeBySiteCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repo.GetEmployeeBySiteAsync(request.SiteId, cancellationToken);
            if (employee == null || !employee.Any())
            {
                return new List<EmployeeSelectDto>();
            }
            return employee.Select(emp => new EmployeeSelectDto
            {
                EmployeeId = emp.EmployeeId,
                EmployeeName = emp.EmployeeName
            }).ToList();


        }

    }
}
