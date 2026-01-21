using AOps.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Infrastructure.Persistence
{
    public class CommonRepository : ICommonRepository
    {
        private readonly AOpsDbContext _context;
        private readonly ILogger<CommonRepository> _logger;
        private readonly IHostEnvironment _env;
        public CommonRepository(AOpsDbContext context, IHostEnvironment env, ILogger<CommonRepository> logger)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
        {
            var basePath = Path.Combine(_env.ContentRootPath, "wwwroot", folder);
            Directory.CreateDirectory(basePath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(basePath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return fileName; // or return a relative path or URL depending on your use case
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folder,string folderName, CancellationToken cancellationToken = default)
        {
            var basePath = Path.Combine(_env.ContentRootPath, "wwwroot", folder,folderName);
            Directory.CreateDirectory(basePath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(basePath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return fileName; // or return a relative path or URL depending on your use case
        }

        public async Task DeleteFileAsync(string fileUrl, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;

            try
            {
                var fileName = Path.GetFileName(fileUrl); // Extract just the filename
                var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", folderName, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to delete file: {fileUrl}");
            }

            await Task.CompletedTask;
        }
    

}
}
