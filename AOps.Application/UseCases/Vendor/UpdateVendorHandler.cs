using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.Vendor
{
    public class UpdateVendorHandler: IRequestHandler<UpdateVendorCommand,int>
    {
        public readonly IVendorRepository _repo;
        public readonly ILogger<UpdateVendorHandler> _loggerRepository;

        public UpdateVendorHandler(IVendorRepository repository, ILogger<UpdateVendorHandler> loggerRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
        }

        public async Task<int> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var mobileExists = await _repo.ExistsMobileAsync(request.VendorMobile, request.VendorId, cancellationToken);
                if (mobileExists)
                {
                    _loggerRepository.LogWarning("Update attempt failed: Mobile number '{Mobile}' already exists for a different vendor.", request.VendorMobile);
                    throw new InvalidOperationException("Mobile already exists.");
                }
                var vendor = new VendorMaster
                {
                    VendorName = request.VendorName,
                    VendorMobile = request.VendorMobile,
                    VendorAddress = request.VendorAddress,
                    VendorEmail = request.VendorEmail,
                    VendorId = request.VendorId,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = request.IsActive

                };
                var id = await _repo.UpdateVendorAsync(vendor, cancellationToken);
                return id;
            }
            catch (Exception ex)
            {
                _loggerRepository.LogError(ex, "Error occurred while updating vendor. VendorId: {VendorId}", request.VendorId);
                throw; // Re-throw to let pipeline handle it if needed
            }
        }
    }
}
