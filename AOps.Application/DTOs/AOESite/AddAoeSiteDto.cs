using AOps.Application.DTOs.DropDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.AOESite
{
    public class AddAoeSiteDto
    {
        public List<Guid> SiteId { get; set; }=new List<Guid>();
        public Guid AoeId { get; set; }
        public List<SiteSelectDto> SiteList { get; set; } = new();
        public List<UsersByRoleDto> ExecutiveList { get; set; } = new();


    }
}
