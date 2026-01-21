using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class EmployeeMasterRepository : IEmployeeMasterRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<EmployeeMasterRepository> _logger;

        public EmployeeMasterRepository(AOpsDbContext context, ILogger<EmployeeMasterRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> AddAsync(EmployeeMaster employee, CancellationToken cancellationToken = default)
        {
            await _context.Employeemaster.AddAsync(employee, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> ExistsEmployeeAsync(string mobile_number, CancellationToken cancellationToken = default)
        {
            return await _context.Employeemaster
                .AnyAsync(v => v.Phone.ToLower() == mobile_number.ToLower() && !v.IsDeleted, cancellationToken);
        }

        public async Task<bool> ExistsEmployeeAsync(string mobile_number, Guid EmployeeId, CancellationToken cancellationToken = default)
        {
            return await _context.Employeemaster
                .AnyAsync(v => v.Phone == mobile_number && v.EmployeeId != EmployeeId, cancellationToken);
        }
        public async Task<IEnumerable<EmployeeMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Employeemaster
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }


        public async Task<EmployeeMaster?> GetByIdAsync(Guid EmployeeId, CancellationToken cancellationToken = default)
        {
            return await _context.Employeemaster
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.EmployeeId == EmployeeId, cancellationToken);
        }

        public async Task<int> UpdateEmployeeAsync(EmployeeMaster employee, CancellationToken cancellationToken = default)
        {
            var emp = await _context.Employeemaster.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId, cancellationToken);
            if (emp == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            // Update fields
            emp.Name = employee.Name;
            emp.UpdatedAt = DateTime.UtcNow;
            emp.Phone = employee.Phone;
            emp.Address = employee.Address;
            emp.Gender = employee.Gender;
            emp.Email = employee.Email;
            emp.DesignationId = employee.DesignationId;
            emp.EmploymentTypeId = employee.EmploymentTypeId;
            emp.IsDeleted = employee.IsDeleted;
            if (!string.IsNullOrWhiteSpace(employee.ProfessionalId))
            {
                emp.ProfessionalId = employee.ProfessionalId;
            }
            if (!string.IsNullOrWhiteSpace(employee.GovernmentId))
            {
                emp.GovernmentId = employee.GovernmentId;
            }
            return await _context.SaveChangesAsync(cancellationToken);

        }

    }
}
