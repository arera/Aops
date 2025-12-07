using AOps.Application.DTOs.Employees;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.VehicleMasters;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.EmployeeMasters
{
    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand,int>
    {
        public readonly IEmployeeMasterRepository _repo;
        public readonly ILogger<AddEmployeeHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;


        public AddEmployeeHandler(IEmployeeMasterRepository repository, ILogger<AddEmployeeHandler> loggerRepository, ICommonRepository commonRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }
        public async Task<int> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Check if employee already exists
            var employeeExists = await _repo.ExistsEmployeeAsync(request.dto.Phone, cancellationToken);
            if (employeeExists)
            {
                throw new Exception("Employee already exists.");
                // Or better: return a ValidationResult instead of throwing
            }

            string? govDocPath = null;
            string? profDocPath = null;
            Guid employeeId = Guid.NewGuid();

            try
            {
                // Step 2: Save government document if provided
                if (request.dto.GovDocumentFile != null)
                {
                    govDocPath = await _commonRepository.SaveFileAsync(
                        request.dto.GovDocumentFile,
                        "EMP_GovernmentID",
                        cancellationToken
                    );
                }

                // Step 3: Save professional document if provided
                if (request.dto.ProfesionalDocumentFile != null)
                {
                    profDocPath = await _commonRepository.SaveFileAsync(
                        request.dto.ProfesionalDocumentFile,
                        "EMP_ProfessionalID",
                        cancellationToken
                    );
                }

                // Step 4: Create employee entity
                var employee = new EmployeeMaster
                {
                    EmployeeId = employeeId,
                    Name = request.dto.Name,
                    Phone = request.dto.Phone,
                    Email = request.dto.Email,
                    Gender = request.dto.Gender,
                    Address = request.dto.Address,
                    DesignationId = request.dto.Designation,
                    EmploymentTypeId = request.dto.EmploymentType,
                    GovernmentId = govDocPath ?? string.Empty,
                    ProfessionalId = profDocPath ?? string.Empty,
                    CreatedAt = DateTime.UtcNow
                };

                // Step 5: Save to DB
                var id = await _repo.AddAsync(employee, cancellationToken);
                return id;
            }
            catch (Exception ex)
            {
                // Step 6: Rollback saved files if something fails
                try
                {
                    if (!string.IsNullOrEmpty(govDocPath) && File.Exists(govDocPath))
                        File.Delete(govDocPath);

                    if (!string.IsNullOrEmpty(profDocPath) && File.Exists(profDocPath))
                        File.Delete(profDocPath);
                }
                catch (Exception deleteEx)
                {
                    _loggerRepository.LogWarning(deleteEx, "Failed to delete uploaded employee files after rollback.");
                }

                _loggerRepository.LogError(ex, "Error inserting employee");
                throw;
            }
        }


    }
}
