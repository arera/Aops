using AOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ILoginRepository
    {
        Task<Orglevels?> GetByUsernameAsync(string Username);
    }
}
