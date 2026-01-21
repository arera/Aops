using AOps.Application.DTOs;
using AOps.Application.DTOs.CustomerLogins;
using AOps.Application.Interfaces;
using AOps.Application.UseCases.LoginUsers;
using AOps.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LoginCustomers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequestCommand,CustomerLoginResponseDto>
    {
        private readonly ICustomerLoginRepository _loginRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher _passwordService;

        public LoginRequestHandler(ICustomerRepository customerRepository, ICustomerLoginRepository loginRepository, IHttpContextAccessor httpContextAccessor, IPasswordHasher passwordService)
        {
            _loginRepository = loginRepository;
            _httpContextAccessor = httpContextAccessor;
            _passwordService = passwordService;
            _customerRepository = customerRepository;
        }
        public async Task<CustomerLoginResponseDto> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _loginRepository.GetByCustomerEmailAsync(request.obj.Email);
            if (user == null || !_passwordService.VerifyPassword(user.PasswordHash, request.obj.Password))
            {
                return new CustomerLoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var customer = await _customerRepository.GetByIdAsync(user.CustomerId, cancellationToken);
            if (customer.IsDeleted)
            {
                return new CustomerLoginResponseDto
                {
                    Success = false,
                    Message = "Account is inactive"
                };
            }

            var roleName = "Customer";

            // 🔹 Build claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.CustomerId.ToString()),  // Guid
        new Claim(ClaimTypes.Email, customer.Email ?? string.Empty),
        new Claim(ClaimTypes.Role, roleName),
        new Claim("CustomerName", customer.Name ?? string.Empty)           // custom claim
    };

            // 🔹 Create identity & principal
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 🔹 Sign in (sets auth cookie)
            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return new CustomerLoginResponseDto
            {
                Success = true,
                CustomerId = user.CustomerId,
                LastLogin = user.LastLogin
            };
        }

    }
}
