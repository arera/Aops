using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AOps.Application.UseCases.VendorContracts
{
    public class AddVendorContractHandler : IRequestHandler<AddVendorContractCommand,int>
    {
        public readonly IVendorContractRepository _repo;
        public readonly ILogger<AddVendorContractCommand> _loggerRepository;
        public readonly ICommonRepository _commonRepository;

        public AddVendorContractHandler(IVendorContractRepository repository,ICommonRepository commonRepository, ILogger<AddVendorContractCommand> loggerRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
            _commonRepository = commonRepository;
        }
        public async Task<int> Handle(AddVendorContractCommand request, CancellationToken cancellationToken)
        {
            string? agreementPath = null;

            try
            {
                // Step 1: Save the file locally
                if (request.obj.AgreementDocument != null)
                {
                    agreementPath = await _commonRepository.SaveFileAsync(
                        request.obj.AgreementDocument,
                        "VendorContractDoc",
                        cancellationToken
                    );
                }

                // Step 2: Create contract entity
                var vcontract = new VendorContract
                {
                    VendorId = request.obj.VendorId,
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
                    ContractId = await _repo.ContractCodeGenerator(request.obj.VendorId)
                };

                // Step 3: Save to DB
                var id = await _repo.AddAsync(vcontract, cancellationToken);
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
