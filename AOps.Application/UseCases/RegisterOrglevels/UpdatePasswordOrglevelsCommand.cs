using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.RegisterOrglevels
{
    public record UpdatePasswordOrglevelsCommand(Guid UserId,string newpassword):IRequest<int>;
   
    
}
