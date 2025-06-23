using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using AOps.Domain.Entities.Common;
using MediatR;

namespace AOps.Application.UseCases.RegisterCustomers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, int>
    {
        private readonly ICustomerRepository _repo;

        public UpdateCustomerHandler(ICustomerRepository repo) => _repo = repo;

        public async Task<int> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _repo.ExistsByEmailAsync(request.Email, request.CustomerId,cancellationToken);
            if (emailExists)
            {
                throw new Exception("Email already exists."); // Or return a validation error accordingly
            }

            var customer = new Customer
            {
                UserId = request.CustomerId,
                Email = request.Email,
                Name = request.Name,
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
                GST = request.GST,
                IsDeleted = request.IsActive,
                UpdatedAt = DateTime.UtcNow
            };

            var id = await _repo.UpdateCustomerAsync(customer, cancellationToken);
            return id;
        }
    }
}
