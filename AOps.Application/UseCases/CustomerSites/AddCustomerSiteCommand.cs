using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerSites
{
    public record AddCustomerSiteCommand(DTOs.CustomerSites.AddCustomerSiteDto Obj):IRequest<int>;
    
}
