using AOps.Application.DTOs.CustomerSites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerSites
{
    public record UpdateCustomerSiteCommand(EditCustomerSiteDto Obj):IRequest<int>;
   
}
