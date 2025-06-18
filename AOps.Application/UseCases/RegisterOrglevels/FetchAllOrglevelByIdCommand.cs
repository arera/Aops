using MediatR;

namespace AOps.Application.UseCases.RegisterOrglevels
{
  
    public record FetchAllOrglevelByIdCommand(Guid UserID) : IRequest<DTOs.GetOrgLevelsDot>;
}
