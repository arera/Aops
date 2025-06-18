using AOps.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterOrglevels
{
    public class UpdatePasswordOrglevelsHandler : IRequestHandler<UpdatePasswordOrglevelsCommand, int>
    {
        private readonly IOrgLevelRepository _repo;
        private readonly IPasswordHasher _passwordService;

        public UpdatePasswordOrglevelsHandler(IOrgLevelRepository repo, IPasswordHasher passwordhasher)
        {
            _repo = repo;
            _passwordService = passwordhasher;
        }

        public async Task<int> Handle(UpdatePasswordOrglevelsCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByUserIdAsync(request.UserId);
            if (user == null)
            {
                return 0; // User not found
            }

            user.Password_hash = _passwordService.HashPassword(request.newpassword);
            await _repo.UpdatePasswordAsync(user.UserID,user.Password_hash);

            return 1; // Password updated successfully
        }
    }
}
