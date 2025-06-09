using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using AOps.Domain.Entities.Common;
using MediatR;


namespace AOps.Application.UseCases.RegisterCustomers
{
    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand, int>
    {
        private readonly ICustomerRepository _repo;

        public RegisterCustomerHandler(ICustomerRepository repo) => _repo = repo;

        public async Task<int> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Address = new Address
                {
                    Street = request.Address.Street,
                    City = request.Address.City,
                    State = request.Address.State,
                    ZipCode = request.Address.ZipCode,
                    Country = request.Address.Country
                },
                PrimaryMobile = request.PrimaryMobile,
                SecondaryMobile = request.SecondaryMobile,
                GST = request.GST
            };


            var id = await _repo.AddAsync(customer, cancellationToken);
            return id;
        }
    }
}
