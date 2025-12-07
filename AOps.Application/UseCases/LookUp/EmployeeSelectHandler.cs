using AOps.Application.DTOs.DropDown;
using AOps.Application.Interfaces;
using MediatR;


namespace AOps.Application.UseCases.LookUp
{
    
   public class EmployeeSelectHandler : IRequestHandler<EmployeeSelectCommand, List<EmployeeSelectDto>>
    {
        private ILookupRepository _repo;

        public EmployeeSelectHandler(ILookupRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<EmployeeSelectDto>> Handle(EmployeeSelectCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repo.GetAllEmployeeAsync(cancellationToken);
            if (employee == null || !employee.Any())
            {
                return new List<EmployeeSelectDto>();
            }
            return employee.Select(emp => new EmployeeSelectDto
            {
                EmployeeId = emp.EmployeeId,
                EmployeeName = emp.EmployeeName
            }).ToList();


        }

    }
}
