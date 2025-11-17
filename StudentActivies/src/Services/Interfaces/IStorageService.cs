using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder = "images");
        Task<bool> DeleteImageAsync(string fileName);
        string GetImageUrl(string fileName);
    }
}
