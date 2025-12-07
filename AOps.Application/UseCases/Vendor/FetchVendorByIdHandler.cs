using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.Vendor
{
    public class FetchVendorByIdHandler: IRequestHandler<FetchVendorByIdCommand, DTOs.Vendor.GetVendorDto>
    {
        private IVendorRepository _repo;

        public FetchVendorByIdHandler(IVendorRepository repo)
        {
            _repo = repo;
        }
        public async Task<DTOs.Vendor.GetVendorDto> Handle(FetchVendorByIdCommand request, CancellationToken cancellationToken)
        {
            var objvendor = await _repo.GetByIdAsync(request.VendorId,cancellationToken);
            if (objvendor == null)
            {
                return null; // or throw an exception if preferred
            }
            return new DTOs.Vendor.GetVendorDto
            {
                VendorName = objvendor.VendorName,
                VendorId = objvendor.VendorId,
                VendorAddress = objvendor.VendorAddress,
                VendorEmail = objvendor.VendorEmail,
                VendorMobile = objvendor.VendorMobile,
                CreatedAt = objvendor.CreatedAt,
                isActive = objvendor.IsDeleted
            };
        }
    }
}
