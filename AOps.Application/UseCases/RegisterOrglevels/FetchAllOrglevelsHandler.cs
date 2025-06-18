using AOps.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterOrglevels
{
    public class FetchAllOrglevelsHandler : IRequestHandler<FetchAllOrglevelsCommand, List<DTOs.GetOrgLevelsDot>>
    {
        private readonly IOrgLevelRepository _repo;

        public FetchAllOrglevelsHandler(IOrgLevelRepository repo)
        {
            _repo = repo;
            
        }
        public async Task<List<DTOs.GetOrgLevelsDot>> Handle(FetchAllOrglevelsCommand request, CancellationToken cancellationToken)
        {
            var orgLevels = await _repo.GetAllAsync(cancellationToken);
            return orgLevels.Select(orgLevel => new DTOs.GetOrgLevelsDot
            {
                UserID = orgLevel.UserID,
                Name = orgLevel.Name,
                Email = orgLevel.Email,
                Role = orgLevel.Role,
                Mobile = orgLevel.Mobile,
                IsActive = orgLevel.IsDeleted  // Assuming IsDeleted is a boolean indicating active status
            }).ToList();
        }
    }
}
