using AOps.Application.DTOs;
using AOps.Application.Interfaces;
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

namespace AOps.Application.UseCases.LoginUsers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher _passwordService;

        public LoginCommandHandler(ILoginRepository loginRepository, IHttpContextAccessor httpContextAccessor, IPasswordHasher passwordService)
        {
            _loginRepository = loginRepository;
            _httpContextAccessor = httpContextAccessor;
            _passwordService = passwordService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _loginRepository.GetByUsernameAsync(request.username);
            if (user == null || !_passwordService.VerifyPassword(user.Password_hash, request.password))
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid username or password"
                };
            }
            if (user.IsDeleted)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Account is inactive"
                };
            }
            var roleName = ((UserRole)user.Role).ToString();

            var claims = new List<Claim>
        {
           new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),     // Guid as string
           new Claim(ClaimTypes.Email, user.Email),
           new Claim(ClaimTypes.Role, roleName)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id.ToString());    // Guid to string
            _httpContextAccessor.HttpContext.Session.SetString("Role", roleName);
            _httpContextAccessor.HttpContext.Session.SetString("Name", user.Name);// int to string


            return new LoginResponseDto
            {
                IsSuccess = true,
                UserId = user.UserID,
                RoleId = user.Role
            };
        }
    }

}
