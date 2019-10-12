using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Zhoplix.Services
{
    public class MediaService
    {
        private readonly ILogger<MediaService> _logger;

        public MediaService(ILogger<MediaService> logger)
        {
            _logger = logger;
        }

        public bool CreatePhotoAsync(UploadPhoto photo)
        {
            try
            {
                using (var image = Image.FromStream(new MemoryStream(photo.Photo)))
                {
                    image.Save($"Images/Uploads/{photo.PhotoId}/{photo.PhotoId}.png", ImageFormat.Png);
                }
                return true;
            }
            catch
            {
                _logger.LogError($"Failed to create image with {photo.PhotoId} id");
                return false;
            }
        }
    }
}
