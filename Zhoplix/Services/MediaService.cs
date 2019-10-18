using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace Zhoplix.Services
{
    public interface IMediaService
    {
        Task CreatePhoto(UploadPhoto photo);
        Task CreateResizedPhoto(string inputPath, string outputPath, float percent);
        Task CreateResizedPhoto(UploadPhoto photo, float percent, string addToName);
        void DeleteAllPhotosWithId(string id);
        void DeletePhoto(string name);
        Task<bool> UploadVideo(IFormFile file);
    }

    public class MediaService : IMediaService
    {
        private readonly ILogger<MediaService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MediaService(ILogger<MediaService> logger,
            IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task CreatePhoto(UploadPhoto photo)
        {
            using var image = Image.Load<Rgba32>(photo.Photo);
            Directory.CreateDirectory($"wwwroot/Images/Uploaded/{photo.PhotoId}");
            await Task.Run(() => image.Save($"wwwroot/Images/Uploaded/{photo.PhotoId}/{photo.PhotoId}.png"));
        }

        public async Task CreateResizedPhoto(UploadPhoto photo, float percent, string addToName)
        {
            Directory.CreateDirectory($"wwwroot/Images/Uploaded/{photo.PhotoId}");
            using var image = Image.Load<Rgba32>(photo.Photo);
            await Task.Run(() =>
            {
                image.Mutate(x => x.Resize((int)(image.Width * percent), (int)(image.Height * percent)));
                image.Save($"wwwroot/Images/Uploaded/{photo.PhotoId}/{photo.PhotoId}_{addToName}.png");
            });
        }

        public async Task CreateResizedPhoto(string inputPath, string outputPath, float percent)
        {
            Directory.CreateDirectory(outputPath);
            using var image = Image.Load<Rgba32>(inputPath);
            await Task.Run(() =>
            {
                image.Mutate(x => x.Resize((int)(image.Width * percent), (int)(image.Height * percent)));
                image.Save(outputPath);
            });
        }

        public void DeleteAllPhotosWithId(string id)
        {
            var di = new DirectoryInfo($"wwwroot/Images/Uploaded/{id}");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            di.Delete();
        }

        public void DeletePhoto(string name)
        {
            var id = name.Split('_')[0];
            File.Delete($"wwwroot/Images/Uploaded/{id}/{name}");
        }

        public async Task<bool> UploadVideo(IFormFile file)
        {
            try
            {
                var folderName = "UploadVideos";
                var webRoot = _hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(webRoot, folderName);
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(newPath, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await Task.Run(() => { file.CopyTo(stream); });
                }

                return true;
            }
            catch(Exception e)
            {
                _logger.LogError($"Failed to create file {file.Name} with exception: {e}");
                return false;
            }
        }
    }
}
