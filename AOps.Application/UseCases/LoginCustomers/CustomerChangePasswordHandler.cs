using AOps.Application.DTOs;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.ChangePassword;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LoginCustomers
{
    
     public class CustomerChangePasswordHandler : IRequestHandler<CustomerChangePasswordCommand, ChangePasswordResponseDto>
    {
        private readonly ICustomerLoginRepository _repo;
        private readonly IPasswordHasher _passwordService;

        public CustomerChangePasswordHandler(ICustomerLoginRepository customerLoginRepository, IPasswordHasher passwordService)
        {
            _repo = customerLoginRepository;
            _passwordService = passwordService;
        }
        public async Task<ChangePasswordResponseDto> Handle(CustomerChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByUserIdAsync(request.obj.CustomerId);
            if (user == null)
            {
                return new ChangePasswordResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Customer not found"
                };
            }

            if (!_passwordService.VerifyPassword(user.PasswordHash, request.obj.OldPassword))
            {
                return new ChangePasswordResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Current password is incorrect"
                };
            }

            user.PasswordHash = _passwordService.HashPassword(request.obj.NewPassword);
            // Assuming userId is the ID of the user making the change
            await _repo.ChangePasswordAsync(user.CustomerId, user.PasswordHash,request.obj.ipAddress);

            return new ChangePasswordResponseDto
            {
                IsSuccess = true
            };
        }
    }
}
