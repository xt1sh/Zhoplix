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

namespace Zhoplix.Services
{
    public interface IMediaService
    {
        void CreatePhoto(UploadPhoto photo);
        void CreateResizedPhoto(string inputPath, string outputPath, float percent);
        void CreateResizedPhoto(UploadPhoto photo, float percent, string addToName);
        void DeleteAllPhotosWithId(int id);
        void DeletePhoto(string name);
    }

    public class MediaService : IMediaService
    {
        private readonly ILogger<MediaService> _logger;

        public MediaService(ILogger<MediaService> logger)
        {
            _logger = logger;
        }

        public void CreatePhoto(UploadPhoto photo)
        {
            using var image = Image.Load<Rgba32>(photo.Photo);
            Directory.CreateDirectory($"wwwroot/Images/Uploaded/{photo.PhotoId}");
            image.Save($"wwwroot/Images/Uploaded/{photo.PhotoId}/{photo.PhotoId}.png");
        }

        public void CreateResizedPhoto(UploadPhoto photo, float percent, string addToName)
        {
            Directory.CreateDirectory($"wwwroot/Images/Uploaded/{photo.PhotoId}");
            using var image = Image.Load<Rgba32>(photo.Photo);
            image.Mutate(x => x.Resize((int)(image.Width * percent), (int)(image.Height * percent)));
            image.Save($"wwwroot/Images/Uploaded/{photo.PhotoId}/{photo.PhotoId}_{addToName}.png");
        }

        public void CreateResizedPhoto(string inputPath, string outputPath, float percent)
        {
            Directory.CreateDirectory(outputPath);
            using var image = Image.Load<Rgba32>(inputPath);
            image.Mutate(x => x.Resize((int)(image.Width * percent), (int)(image.Height * percent)));
            image.Save(outputPath);
        }

        public void DeleteAllPhotosWithId(int id)
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
    }
}
