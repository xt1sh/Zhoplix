﻿using Microsoft.Extensions.Logging;
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
        string UploadImagesPath { get; }
        string UploadVideosPath { get; }

        Task CreatePhoto(UploadPhoto photo);
        Task CreateResizedPhoto(string inputPath, string outputPath, float percent);
        Task CreateResizedPhoto(UploadPhoto photo, float percent, string addToName);
        void DeleteAllPhotosWithId(string id);
        void DeletePhoto(string name);
        Task<bool> UploadVideo(IFormFile file, string id);
    }

    public class MediaService : IMediaService
    {
        public string UploadImagesPath { get; }
        public string UploadVideosPath { get; }

        private readonly ILogger<MediaService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MediaService(ILogger<MediaService> logger,
            IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            UploadImagesPath = Path.Combine(hostingEnvironment.WebRootPath, "Images", "Uploaded");
            UploadVideosPath = Path.Combine(hostingEnvironment.WebRootPath, "Videos", "Uploaded");
        }

        public async Task CreatePhoto(UploadPhoto photo)
        {
            using var image = Image.Load<Rgba32>(photo.Photo);
            var folder = Path.Combine(UploadImagesPath, photo.PhotoId);
            Directory.CreateDirectory(folder);
            var path = Path.ChangeExtension(Path.Combine(folder, photo.PhotoId), "png");
            await Task.Run(() => image.Save(path));
        }

        public async Task CreateResizedPhoto(UploadPhoto photo, float percent, string addToName)
        {
            Directory.CreateDirectory(Path.Combine(UploadImagesPath, photo.PhotoId));
            var folder = Path.Combine(UploadImagesPath, photo.PhotoId);
            using var image = Image.Load<Rgba32>(photo.Photo);
            var path = Path.ChangeExtension(Path.Combine(folder, $"{photo.PhotoId}_{addToName}"), "png");
            await Task.Run(() =>
            {
                image.Mutate(x => x.Resize((int)(image.Width * percent), (int)(image.Height * percent)));
                image.Save(path);
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
            var di = new DirectoryInfo(Path.Combine(UploadImagesPath, id));

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            di.Delete();
        }

        public void DeletePhoto(string name)
        {
            var id = name.Split('_')[0];
            File.Delete(Path.Combine(UploadImagesPath, id, name));
        }

        public async Task<bool> UploadVideo(IFormFile file, string id)
        {
            try
            {
                Directory.CreateDirectory(UploadVideosPath);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(UploadVideosPath, id);
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

        public void RenameVideo(string path, string newName)
        {
            var info = new FileInfo(path);
            var directory = Path.GetDirectoryName(path);
            File.Move(path, Path.ChangeExtension(Path.Combine(directory, newName), info.Extension));
        }

        public void RenameUploadedVideo(string name, string newName)
        {
            var path = Path.Combine(UploadVideosPath, name);
            var info = new FileInfo(path);
            RenameVideo(info.FullName, newName);
        }
    }
}
