using AOps.Application.DTOs.DropDown;
using AOps.Application.DTOs.Vendor;
using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.Vendor
{
    public class CustomerDropDownHandler : IRequestHandler<VendorDropDownCommand, List<DTOs.Vendor.VendorDropdownDto>>
    {
        private IVendorRepository _repo;

        public CustomerDropDownHandler(IVendorRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<DTOs.Vendor.VendorDropdownDto>> Handle(VendorDropDownCommand request, CancellationToken cancellationToken)
        {
            var vendors = await _repo.GetVendorDropdownAsync(cancellationToken);
            if (vendors == null || !vendors.Any())
            {
                return new List<VendorDropdownDto>();
            }
            return vendors.Select(vendor => new DTOs.Vendor.VendorDropdownDto
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
            }).ToList();
        }
    }
}
