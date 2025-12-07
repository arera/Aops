using AOps.Application.DTOs.DropDown;
using AOps.Application.DTOs.EmpContract;
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
    public class EmployeeContractRepository : IEmployeeContractRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<EmployeeMasterRepository> _logger;

        public EmployeeContractRepository(AOpsDbContext context, ILogger<EmployeeMasterRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Add new contract
        public async Task<int> AddAsync(EmployeeContract ccontract, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.EmployeeContracts.AddAsync(ccontract, cancellationToken);
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding employee contract");
                throw;
            }
        }

        public Task<IEnumerable<FetchAllEmployeeContractDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        // Get contract by Id
        public async Task<EmployeeContract?> GetByIdAsync(string contract_id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.EmployeeContracts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ContractId.ToString() == contract_id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching contract by ID {ContractId}", contract_id);
                throw;
            }
        }

        // Update existing contract
        public async Task<int> UpdateContractAsync(EmployeeContract ccontract, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingContract = await _context.EmployeeContracts
                    .FirstOrDefaultAsync(x => x.ContractId == ccontract.ContractId, cancellationToken);

                if (existingContract == null)
                    throw new KeyNotFoundException("Contract not found");

                existingContract.EmployeeId = ccontract.EmployeeId;
                existingContract.ContractStartDate = ccontract.ContractStartDate;
                existingContract.ContractEndDate = ccontract.ContractEndDate;
                existingContract.Description = ccontract.Description;
                existingContract.ContractDocument = ccontract.ContractDocument; // if applicable

                _context.EmployeeContracts.Update(existingContract);
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating employee contract");
                throw;
            }
        }

        // Get employee dropdown list for a specific customer
       

    }
}
