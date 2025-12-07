using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Interfaces
{
    public interface ICommonRepository
    {
        Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
        Task<string> SaveFileAsync(IFormFile file, string folder,string folderName, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string fileUrl, string folderName);
    }

}
