using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using AOps.Domain.Entities.Common;
using AOps.Domain.Enums;
using MediatR;


namespace AOps.Application.UseCases.RegisterOrglevels
{
    public class RegisterOrglevelsHandler : IRequestHandler<RegisterOrgLevelsCommand, int>
    {
        private readonly IOrgLevelRepository _repo;
        private readonly IPasswordHasher _passwordService;

        public RegisterOrglevelsHandler(IOrgLevelRepository repo, IPasswordHasher passwordhasher)
        {
            _repo = repo;
            _passwordService = passwordhasher;
        }

        public async Task<int> Handle(RegisterOrgLevelsCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _repo.ExistsByEmailAsync(request.Email, cancellationToken);
            if (emailExists)
            {
                throw new Exception("Email already exists."); // Or return a validation error accordingly
            }
            var orgLevel = new Orglevels
            {
                Name = request.Name,
                Email = request.Email,
                Role = request.Role,
                Mobile = request.Mobile,
                Password_hash = _passwordService.HashPassword(request.passwordhash) // replace with your hashing logic
            };
            var id = await _repo.AddAsync(orgLevel, cancellationToken);
            return id;
        }
    }
}
