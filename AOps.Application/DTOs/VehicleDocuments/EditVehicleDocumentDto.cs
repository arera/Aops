using AOps.Application.DTOs.Vehicle;
using AOps.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.DTOs.VehicleDocuments
{
    public class EditVehicleDocumentDto
    {
        public Guid DocumentId { get; set; }
        public Guid VehicleId { get; set; }

        public string DocumentType { get; set; } = string.Empty;

        public string? OldFileUrl { get; set; }

        public VehicleDocumentType DocumentList { get; set; }

        public DateTime ExpiryDate { get; set; }

        public IFormFile? DocumentFile { get; set; }

        public List<VehicleDropdownDto> VehicleList { get; set; } = new();
    }
}
