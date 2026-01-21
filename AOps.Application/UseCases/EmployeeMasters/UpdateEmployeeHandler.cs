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
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, int>
    {
        public readonly IEmployeeMasterRepository _repo;
        public readonly ILogger<UpdateEmployeeCommand> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public UpdateEmployeeHandler(IEmployeeMasterRepository repository, ILogger<UpdateEmployeeCommand> loggerRepository, ICommonRepository commonRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }
        public async Task<int> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeExists = await _repo.ExistsEmployeeAsync(
                request.obj.Phone,
                request.obj.EmployeeId,
                cancellationToken
            );

            if (employeeExists)
            {
                throw new Exception("Employee already exists.");
                // Better: return a validation result instead of throwing
            }

            string? govDocPath = null;
            string? profDocPath = null;

            try
            {
                // Step 1: Get existing employee
                var employee = await _repo.GetByIdAsync(request.obj.EmployeeId, cancellationToken);
                if (employee == null)
                {
                    throw new Exception($"Employee with ID {request.obj.EmployeeId} not found.");
                }

                // Step 2: Handle government document upload
                if (request.obj.GovDocumentFile != null)
                {
                    govDocPath = await _commonRepository.SaveFileAsync(
                        request.obj.GovDocumentFile,
                        "EMP_GovernmentID",
                        cancellationToken
                    );

                }

                // Step 3: Handle professional document upload
                if (request.obj.ProfessionalDocumentFile != null)
                {
                    profDocPath = await _commonRepository.SaveFileAsync(
                        request.obj.ProfessionalDocumentFile,
                        "EMP_ProfessionalID",
                        cancellationToken
                    );
                }

                // Step 4: Update other fields
                employee.Name = request.obj.Name;
                employee.Phone = request.obj.Phone;
                employee.Email = request.obj.Email;
                employee.Gender = request.obj.Gender;
                employee.Address = request.obj.Address;
                employee.DesignationId = request.obj.Designation;       
                employee.EmploymentTypeId = request.obj.EmploymentType; 
                employee.UpdatedAt = DateTime.UtcNow;
                employee.IsDeleted = request.obj.EStatus;
                employee.GovernmentId = govDocPath ?? string.Empty;
                employee.ProfessionalId = profDocPath ?? string.Empty;

                // Step 5: Save to DB
                var id = await _repo.UpdateEmployeeAsync(employee, cancellationToken);

                // Step 6: Delete old files if replaced
                if (!string.IsNullOrWhiteSpace(govDocPath))
                {
                    var folderPath = Path.Combine("EMP_GovernmentID", request.obj.EmployeeId.ToString());
                    await _commonRepository.DeleteFileAsync(govDocPath, folderPath);
                }

                if (!string.IsNullOrWhiteSpace(profDocPath))
                {
                    var folderPath = Path.Combine("EMP_ProfessionalID", request.obj.EmployeeId.ToString());
                    await _commonRepository.DeleteFileAsync(profDocPath, folderPath);
                }

                return id;
            }
            catch (Exception ex)
            {
                // Rollback uploaded files if error occurs
                try
                {
                    if (!string.IsNullOrEmpty(govDocPath))
                    {
                        var folderPath = Path.Combine("EMP_GovernmentID", request.obj.EmployeeId.ToString());
                        await _commonRepository.DeleteFileAsync(govDocPath, folderPath);
                    }

                    if (!string.IsNullOrEmpty(profDocPath))
                    {
                        var folderPath = Path.Combine("EMP_ProfessionalID", request.obj.EmployeeId.ToString());
                        await _commonRepository.DeleteFileAsync(profDocPath, folderPath);
                    }
                }
                catch (Exception deleteEx)
                {
                    _loggerRepository.LogWarning(deleteEx, "Failed to rollback uploaded employee files.");
                }

                _loggerRepository.LogError(ex, "Error updating employee record");
                throw;
            }
        }

    }
}
