using AOps.Application.DTOs.AOESite;
using AOps.Application.DTOs.CustomerTickets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AOESites
{
   public record AddAOEMappingCommand(AddAoeSiteDto obj) : IRequest<Guid>;
}
