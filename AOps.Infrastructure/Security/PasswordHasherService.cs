// AOps.Infrastructure/Security/PasswordHasherService.cs
using AOps.Application.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace AOps.Infrastructure.Security
{
    public class PasswordHasherService : IPasswordHasher
    {
        private readonly IPasswordHasher<object> _hasher;

       
        public PasswordHasherService(IPasswordHasher<object> hasher)
        {
            _hasher = hasher;
        }

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
