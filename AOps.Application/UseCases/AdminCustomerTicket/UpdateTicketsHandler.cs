using AOps.Application.Interfaces;
using AOps.Application.UseCases.CustomerTicket;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.AdminCustomerTicket
{
        public class UpdateTicketsHandler : IRequestHandler<UpdateTicketsCommand, int>
        {
            private readonly IAdminCustomerTicket _repo;
            private readonly ILogger<UpdateTicketsHandler> _logger;

            public UpdateTicketsHandler(
                IAdminCustomerTicket repo,
                ILogger<UpdateTicketsHandler> logger)
            {
                _repo = repo;
                _logger = logger;
            }

            public async Task<int> Handle(UpdateTicketsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _repo.UpdateTicketAsync(request.dto, cancellationToken);

                    if (result == 0)
                    {
                        _logger.LogWarning("No ticket was updated. TicketId: {TicketId}", request.dto.TicketId);
                    }
                    else
                    {
                        _logger.LogInformation("Ticket updated successfully. TicketId: {TicketId}", request.dto.TicketId);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while updating ticket {TicketId}", request.dto.TicketId);
                    throw;
                }
            }
        }

    }


