using AOps.Application.DTOs.Employees;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.EmployeeMasters
{
    public class FetchEmployeeHandler:IRequestHandler<FetchEmployeeCommand,List<FetchEmployeesDto>>
    {
        public readonly IEmployeeMasterRepository _repo;
        public readonly ILogger<FetchEmployeeHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;
        private readonly string _siteUrl;


        public FetchEmployeeHandler(IEmployeeMasterRepository repository, ILogger<FetchEmployeeHandler> loggerRepository, ICommonRepository commonRepository, string siteUrl)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
            _siteUrl = siteUrl;
        }
        public async Task<List<FetchEmployeesDto>> Handle(FetchEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employees = await _repo.GetAllAsync(cancellationToken);

            return employees.Select(emp => new FetchEmployeesDto
            {
                EmployeeId = emp.EmployeeId,
                Name = emp.Name,
                Phone = emp.Phone,
                Email = emp.Email,
                DateOfBirth = emp.DateOfBirth,
                Gender = emp.Gender,
                Address = emp.Address,
                Designation = emp.DesignationId,       
                EmploymentType = emp.EmploymentTypeId,
                EStatus = emp.IsDeleted,
                GovernmentId = !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(emp.GovernmentId)
                    ? $"{_siteUrl.TrimEnd('/')}/EMP_GovernmentID/{Path.GetFileName(emp.GovernmentId)}"
                    : null,

                ProfessionalId = !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(emp.ProfessionalId)
                    ? $"{_siteUrl.TrimEnd('/')}/EMP_ProfessionalID/{Path.GetFileName(emp.ProfessionalId)}"
                    : null
            }).ToList();
        }

    }
}
