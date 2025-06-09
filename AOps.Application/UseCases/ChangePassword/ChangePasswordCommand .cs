using AOps.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.ChangePassword
{
    public record ChangePasswordCommand(Guid userId, string currentPassword, string newPassword) : IRequest<ChangePasswordResponseDto>;
   
}
