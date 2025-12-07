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
    public class AddVendorHandler : IRequestHandler<AddVendorCommand,int>
    {
        public readonly IVendorRepository _repo;
        public readonly ILogger<AddVendorCommand> _loggerRepository;

        public AddVendorHandler(IVendorRepository repository, ILogger<AddVendorCommand> loggerRepository)
        {
            _repo = repository;
            _loggerRepository = loggerRepository;
        }

        public async Task<int> Handle(AddVendorCommand request, CancellationToken cancellationToken)
        {
            var mobileExists = await _repo.ExistsMobileAsync(request.VendorMobile, cancellationToken);
            if (mobileExists)
            {
                throw new Exception("Mobile already exists."); // Or return a validation error accordingly
            }
            var vendor = new VendorMaster
            {
                VendorName = request.VendorName,
                VendorMobile = request.VendorMobile,
                VendorAddress = request.VendorAddress,
                VendorEmail = request.VendorEmail,
                VendorId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
               
            };
            var id = await _repo.AddAsync(vendor, cancellationToken);
            return id;
        }
    }
}
