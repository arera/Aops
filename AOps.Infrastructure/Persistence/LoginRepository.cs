using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AOps.Infrastructure.Persistence
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<LoginRepository> _logger;

        public LoginRepository(AOpsDbContext context, ILogger<LoginRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Orglevels?> GetByUsernameAsync(string username)
        {
            return await _context.OrganisationLevels.FirstOrDefaultAsync(u => u.Email == username || u.Mobile == username);
        }
    }
}
