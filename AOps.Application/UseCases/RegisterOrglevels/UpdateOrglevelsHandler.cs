using AOps.Application.Interfaces;
using AOps.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterOrglevels
{
    public class UpdateOrglevelsHandler : IRequestHandler<UpdateOrglevelsCommand, int>
    {
        private IOrgLevelRepository _repo;
        public UpdateOrglevelsHandler(IOrgLevelRepository rep)
        {
            _repo = rep;
        }
        public async Task<int> Handle(UpdateOrglevelsCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _repo.ExistsByEmailAsync(request.Email,request.UserId, cancellationToken);
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
                UserID = request.UserId,
                IsDeleted = request.isActive
            };
            var id = await _repo.UpdateAsync(orgLevel, cancellationToken);
            return id;
        }
    }
    
}
