using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.CustomerSites
{
    public record FetchCustomerSiteByIdCommand(Guid Siteid):IRequest<DTOs.CustomerSites.GetCustomerSiteDto>;
}
