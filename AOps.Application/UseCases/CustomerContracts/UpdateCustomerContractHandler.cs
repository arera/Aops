using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerContracts
{
    public class UpdateCustomerContractHandler : IRequestHandler<UpdateCustomerContractCommand, int>
    {
        public readonly ICustomerContractRepository _repo;
        public readonly ILogger<UpdateCustomerContractHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public UpdateCustomerContractHandler(ICustomerContractRepository repo, ILogger<UpdateCustomerContractHandler> logger, ICommonRepository commonrepository)
        {
            _repo = repo;
            _loggerRepository = logger;
            _commonRepository = commonrepository;
        }

        public async Task<int> Handle(UpdateCustomerContractCommand request, CancellationToken cancellationToken)
        {
            string? agreementPath = null;
            string? oldFileToDelete = null;

            try
            {
                // Step 1: Save new file (if uploaded)
                if (request.obj.AgreementDocument != null)
                {
                    agreementPath = await _commonRepository.SaveFileAsync(
                        request.obj.AgreementDocument,
                        "CustomerContractDoc",
                        cancellationToken
                    );

                    if (!string.IsNullOrWhiteSpace(request.obj.AgreementDocumentPath))
                    {
                        oldFileToDelete = request.obj.AgreementDocumentPath;
                    }
                }

                // Step 2: Prepare entity for update
                var vcontract = new CustomerContract
                {
                    CustomerId = request.obj.CustomerId,
                    ContractDetails = request.obj.ContractDetails,
                    ContractValue = request.obj.ContractValue,
                    ContractType = request.obj.ContractType,
                    AgreementDocument = agreementPath ?? string.Empty,
                    StaffAgreed = request.obj.StaffAgreed,
                    StaffsUsed = request.obj.StaffUsed,
                    VehiclesAgreed = request.obj.VehiclesAgreed,
                    VehiclesUsed = request.obj.VehiclesUsed,
                    StartDate = request.obj.StartDate,
                    EndDate = request.obj.EndDate,
                    ContractId = request.obj.ContractId
                };

                // Step 3: Update DB record
                var result = await _repo.UpdateContractAsync(vcontract, cancellationToken);

                if (!string.IsNullOrWhiteSpace(oldFileToDelete))
                {
                    await _commonRepository.DeleteFileAsync(oldFileToDelete, "CustomerContractDoc");
                }

                return result;


            }
            catch (Exception ex)
            {
                // Cleanup if new file was saved but an error occurred
                if (!string.IsNullOrEmpty(agreementPath))
                {
                    try
                    {
                        if (File.Exists(agreementPath))
                        {
                            File.Delete(agreementPath);
                        }
                    }
                    catch (Exception delEx)
                    {
                        _loggerRepository.LogWarning(delEx, $"Failed to delete uploaded file at: {agreementPath}");
                    }
                }

                _loggerRepository.LogError(ex, "Error updating vendor contract");
                throw;
            }
        }

    }
}
