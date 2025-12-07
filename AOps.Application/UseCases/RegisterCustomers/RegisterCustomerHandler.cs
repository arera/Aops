using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using AOps.Domain.Entities.Common;
using MediatR;



namespace AOps.Application.UseCases.RegisterCustomers
{
    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repo;
        private readonly ICustomerLoginRepository _loginRepository;
        private readonly IPasswordHasher _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerHandler(ICustomerRepository repo,ICustomerLoginRepository customerLoginRepository,IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _repo = repo;
            _loginRepository = customerLoginRepository;
            _passwordService = passwordHasher;
            _unitOfWork = unitOfWork;
        }


        public async Task<Guid> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _repo.ExistsByEmailAsync(request.Email, cancellationToken);
            if (emailExists)
            {
                throw new Exception("Email already exists.");
            }

            var customerId = Guid.NewGuid();

            var customer = new Customer
            {
                UserId = customerId,
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

            //var plainPassword = Common.Utility.PasswordGenerator.GenerateRandomPassword(12);
            var plainPassword = "Admin@123";
            var hashedPassword = _passwordService.HashPassword(plainPassword);

            var login = new CustomerLogin
            {
                CustomerId = customerId,
                PasswordHash = hashedPassword,
                LastLogin = DateTime.UtcNow,
                IpAddress = string.Empty,
            };

            // Transaction ensures both inserts succeed or none
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _repo.AddAsync(customer, cancellationToken);
                await _loginRepository.AddAsync(login);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                // TODO: send plainPassword via email/SMS here

                return customerId;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw; // bubble up exception
            }
        }

    }
}
