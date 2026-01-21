using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.UseCases.UploadVehicleDocuments
{
    public record AddVehicleDocumentsCommand(DTOs.VehicleDocuments.AddVehicleDocumentDto obj):IRequest<int>;
   
}
