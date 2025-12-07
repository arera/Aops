using AOps.Application.Interfaces;
using AOps.Application.UseCases.RegisterOrglevels;
using MediatR;

namespace AOps.Application.UseCases.Vendor
{
    public class FetchVendorHandler : IRequestHandler<FetchAllVendorCommand,List<DTOs.Vendor.GetVendorDto>>
    {
        private IVendorRepository _repo;

        public FetchVendorHandler(IVendorRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<DTOs.Vendor.GetVendorDto>> Handle(FetchAllVendorCommand request, CancellationToken cancellationToken)
        {
            var vendors = await _repo.GetAllAsync(cancellationToken);
            return vendors.Select(vendor => new DTOs.Vendor.GetVendorDto
            {
               VendorId = vendor.VendorId,
               VendorName = vendor.VendorName,
               VendorAddress = vendor.VendorAddress,
               VendorEmail = vendor.VendorEmail,
               VendorMobile = vendor.VendorMobile,
               CreatedAt = vendor.CreatedAt,
               isActive = vendor.IsDeleted
            }).ToList();
        }
    }
}
