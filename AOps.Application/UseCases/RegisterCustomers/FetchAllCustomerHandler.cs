using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterCustomers
{
    public class FetchAllCustomerHandler : IRequestHandler<FetchAllCustomerCommand, List<DTOs.Customer.CreateCustomerDto>>
    {
        private readonly ICustomerRepository _repo;

        public FetchAllCustomerHandler(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DTOs.Customer.CreateCustomerDto>> Handle(FetchAllCustomerCommand request, CancellationToken cancellationToken)
        {
            var customers = await _repo.GetAllAsync(cancellationToken);
            return customers.Select(customer => new DTOs.Customer.CreateCustomerDto
            {
                CustomerId = customer.UserId,
                Name = customer.Name,
                Email = customer.Email,
                GST = customer.GST, // Assuming Role is represented by GST Number
                PrimaryMobile = customer.PrimaryMobile,
                SecondaryMobile = customer.SecondaryMobile,
                Address = new DTOs.Customer.AddressDto
                {
                    Street = customer.Address.Street,
                    City = customer.Address.City,
                    State = customer.Address.State,
                    ZipCode = customer.Address.ZipCode,
                    Country = customer.Address.Country
                },
                IsActive = customer.IsDeleted  // Assuming IsDeleted is a boolean indicating active status
            }).ToList();
        }
    }
}
