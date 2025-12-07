using AOps.Application.DTOs.Employees;
using AOps.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.EmployeeMasters
{
    
     public class FetchEmployeeByIdHandler : IRequestHandler<FetchEmployeeByIdCommand, FetchEmployeesDto>
    {
        public readonly IEmployeeMasterRepository _repo;
        public readonly ILogger<FetchEmployeeByIdHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;
        private readonly string _siteUrl;


        public FetchEmployeeByIdHandler(IEmployeeMasterRepository repository, ILogger<FetchEmployeeByIdHandler> loggerRepository, ICommonRepository commonRepository, string siteUrl)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
            _siteUrl = siteUrl;
        }
        public async Task<FetchEmployeesDto> Handle(FetchEmployeeByIdCommand request, CancellationToken cancellationToken)
        {
            var emp = await _repo.GetByIdAsync(request.EmployeeId, cancellationToken);

            if (emp == null)
                return null; // or throw exception if needed

            return new FetchEmployeesDto
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
                    ? $"{_siteUrl.TrimEnd('/')}/EMP_GovernmentID/{emp.Id}/{Path.GetFileName(emp.GovernmentId)}"
                    : null,

                ProfessionalId = !string.IsNullOrWhiteSpace(_siteUrl) && !string.IsNullOrWhiteSpace(emp.ProfessionalId)
                    ? $"{_siteUrl.TrimEnd('/')}/EMP_ProfessionalID/{emp.Id}/{Path.GetFileName(emp.ProfessionalId)}"
                    : null
            };
        }


    }
}
