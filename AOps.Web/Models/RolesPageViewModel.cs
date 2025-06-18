using AOps.Application.DTOs;

namespace AOps.Web.Models
{
    public class RolesPageViewModel
    {
        public IEnumerable<GetOrgLevelsDot> ExistingRoles { get; set; } = new List<GetOrgLevelsDot>();
        public RegisterOrgLevelsDot NewUser { get; set; } = new RegisterOrgLevelsDot();
    }
}
