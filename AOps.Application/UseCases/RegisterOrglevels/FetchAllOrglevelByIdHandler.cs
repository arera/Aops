using AOps.Application.Interfaces;
using MediatR;


namespace AOps.Application.UseCases.RegisterOrglevels
{
    public class FetchAllOrglevelByIdHandler : IRequestHandler<FetchAllOrglevelByIdCommand, DTOs.GetOrgLevelsDot>
    {
        private readonly IOrgLevelRepository _repo;

        public FetchAllOrglevelByIdHandler(IOrgLevelRepository repo)
        {
            _repo = repo;
        }

        public async Task<DTOs.GetOrgLevelsDot> Handle(FetchAllOrglevelByIdCommand request, CancellationToken cancellationToken)
        {
            var orgLevel = await _repo.GetByUserIdAsync(request.UserID, cancellationToken);
            if (orgLevel == null)
            {
                return null; // or throw an exception if preferred
            }
            return new DTOs.GetOrgLevelsDot
            {
                UserID = orgLevel.UserID,
                Name = orgLevel.Name,
                Email = orgLevel.Email,
                Role = orgLevel.Role,
                Mobile = orgLevel.Mobile
            };
        }
    }
}
