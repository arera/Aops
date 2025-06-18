using MediatR;


namespace AOps.Application.UseCases.RegisterOrglevels
{
        public record UpdateOrglevelsCommand(string Name, string Email, int Role, string? Mobile, Guid UserId,Boolean isActive) : IRequest<int>;
}
