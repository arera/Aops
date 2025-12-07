using AOps.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.UploadVehicleDocuments
{
    public record FetchVehicleDocumentsCommand():IRequest<List<DTOs.VehicleDocuments.FetchVehicleDocumentsDto>>;
   
}
