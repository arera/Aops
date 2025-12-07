using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.CustomerContracts
{
    public class AddCustomerContractHandler : IRequestHandler<AddCustomerContractCommand,int>
    {
        public readonly ICustomerContractRepository _repo;
        public readonly ILogger<AddCustomerContractHandler> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public AddCustomerContractHandler(ICustomerContractRepository repository,ICommonRepository commonRepository, ILogger<AddCustomerContractHandler> loggerRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }
        public async Task<int> Handle(AddCustomerContractCommand request, CancellationToken cancellationToken)
        {
            string? agreementPath = null;

            try
            {
                // Step 1: Save the file locally
                if (request.obj.AgreementDocument != null)
                {
                    agreementPath = await _commonRepository.SaveFileAsync(
                        request.obj.AgreementDocument,
                        "CustomerContractDoc",
                        cancellationToken
                    );
                }

                // Step 2: Create contract entity
                var ccontract = new CustomerContract
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
                    ContractId = await _repo.ContractCodeGenerator(request.obj.CustomerId)
                };

                // Step 3: Save to DB
                var id = await _repo.AddAsync(ccontract, cancellationToken);
                return id;
            }
            catch (Exception ex)
            {
                // Step 4: Delete the file from local storage if it was saved
                if (!string.IsNullOrEmpty(agreementPath))
                {
                    try
                    {
                        if (File.Exists(agreementPath))
                        {
                            File.Delete(agreementPath);
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        _loggerRepository.LogWarning(deleteEx, $"Failed to delete uploaded file at: {agreementPath}");
                    }
                }

                // Log and rethrow the original exception
                _loggerRepository.LogError(ex, "Error inserting vendor contract");
                throw;
            }
        }

    }
}
