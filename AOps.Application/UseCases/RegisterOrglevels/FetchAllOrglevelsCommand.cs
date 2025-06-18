using MediatR;

namespace AOps.Application.UseCases.RegisterOrglevels
{
    public record FetchAllOrglevelsCommand : IRequest<List<DTOs.GetOrgLevelsDot>>;
    
}
