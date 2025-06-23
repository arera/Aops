using AOps.Application.Interfaces;
using MediatR;

namespace AOps.Application.UseCases.RegisterCustomers
{
    public class FetchAllCustomerByIdHandler : IRequestHandler<FetchAllCustomerByIdCommand, DTOs.Customer.CreateCustomerDto>
    {
        private readonly ICustomerRepository _repo;

        public FetchAllCustomerByIdHandler(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<DTOs.Customer.CreateCustomerDto> Handle(FetchAllCustomerByIdCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repo.GetByIdAsync(request.Userid, cancellationToken);
            if (customer == null)
            {
                return null; // or throw an exception if preferred
            }
            return new DTOs.Customer.CreateCustomerDto
            {
                CustomerId = customer.UserId,
                Name = customer.Name,
                Email = customer.Email,
                PrimaryMobile = customer.PrimaryMobile,
                SecondaryMobile = customer.SecondaryMobile,
                IsActive = !customer.IsDeleted, 
                GST = customer.GST,
                Address = new DTOs.Customer.AddressDto
                {
                    Street = customer.Address.Street,
                    City = customer.Address.City,
                    State = customer.Address.State,
                    ZipCode = customer.Address.ZipCode,
                    Country = customer.Address.Country
                },
            };
        }
    }
}
