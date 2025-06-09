using AOps.Application.DTOs;
using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand,ChangePasswordResponseDto>
    {
        private readonly IOrgLevelRepository _orgLevelRepository;
        private readonly IPasswordHasher _passwordService;

        public ChangePasswordHandler(IOrgLevelRepository orgLevelRepository, IPasswordHasher passwordService)
        {
            _orgLevelRepository = orgLevelRepository;
            _passwordService = passwordService;
        }
        public async Task<ChangePasswordResponseDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _orgLevelRepository.GetByUserIdAsync(request.userId);
            if (user == null)
            {
                return new ChangePasswordResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found"
                };
            }

            if (!_passwordService.VerifyPassword(user.Password_hash, request.currentPassword))
            {
                return new ChangePasswordResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Current password is incorrect"
                };
            }

            user.Password_hash = _passwordService.HashPassword(request.newPassword);
            // Assuming userId is the ID of the user making the change
            await _orgLevelRepository.UpdatePasswordAsync(user.UserID,user.Password_hash);

            return new ChangePasswordResponseDto
            {
                IsSuccess = true
            };
        }
    }
}
