using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AOps.Web.Common
{
    public static class FileUploadHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string rootPath, string folder = "uploads")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is missing.");

            string uploadsFolder = Path.Combine(rootPath, folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Create a unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path or just the filename
            return uniqueFileName;
        }
    }
}
