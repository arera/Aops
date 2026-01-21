using AOps.Application.DTOs.DropDown;
using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.LookUp
{
     public class UserSelectHandler : IRequestHandler<UserSelectCommand, List<UsersByRoleDto>>
    {
        private ILookupRepository _repo;

        public UserSelectHandler(ILookupRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<UsersByRoleDto>> Handle(UserSelectCommand request, CancellationToken cancellationToken)
        {
            var users = await _repo.GetUsersByRoleAsync(request.RoleId, cancellationToken);
            if (users == null || !users.Any())
            {
                return new List<UsersByRoleDto>();
            }
            return users.Select(site => new UsersByRoleDto
            {
                UserId = site.UserId,
                UserName = site.UserName,
            }).ToList();
        }
    }
}
