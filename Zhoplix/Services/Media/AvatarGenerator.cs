using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Services.Media
{
    public interface IAvatarGenerator
    {
        string ImagePath { get; }
        string GenerateAvatar(int size, int resolutionSize);
    }

    public class AvatarGenerator : IAvatarGenerator
    {
        public string ImagePath { get; }

        public AvatarGenerator(IHostEnvironment hostEnvironment)
        {
            ImagePath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "Images", "Avatars");
        }

        public string GenerateAvatar(int size, int resolutionSize)
        {
            byte[][] pixels = new byte[size][];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new byte[size];
            }

            var random = new Random(DateTime.Now.Millisecond);

            int center = size % 2 == 0 ? size / 2 : size / 2 + 1;

            for (int i = 0; i < center; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    pixels[i][j] = (byte)random.Next(0, 2);
                }
            }

            for (int i = center; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    pixels[i][j] = pixels[size - i - 1][j];
                }
            }

            List<string> colors = new List<string> { "#800000", "#DC143C", "#FF4500", "#FF8C00", "#FFD700", "#808000", "#006400",
                "#00FF00", "#00FFFF", "#000080", "#4B0082", "#8B008B", "#FF1493", "#D2691E" };

            var color = Rgba32.FromHex(colors[random.Next(0, colors.Count)]);

            var id = Guid.NewGuid().ToString();

            using (var image = new Image<Rgba32>(resolutionSize, resolutionSize))
            {
                var pixelSize = resolutionSize / size;
                var pixeli = -1;
                var pixelj = -1;
                for (int i = 0; i < resolutionSize; i++)
                {
                    if (i % pixelSize == 0)
                        pixeli++;

                    for (int j = 0; j < resolutionSize; j++)
                    {
                        if (j % pixelSize == 0)
                            pixelj++;

                        if (pixels[pixeli][pixelj] == 1)
                            image[i, j] = color;
                    }
                    pixelj = -1;
                }

                Directory.CreateDirectory(Path.Combine(ImagePath, id));

                image.Save(Path.Combine(ImagePath, id, $"{id}.png"));
            }

            using (var image = Image.Load(Path.Combine(ImagePath, id, $"{id}.png")))
            {
                image.Mutate(x => x.Resize(resolutionSize / 2, resolutionSize / 2));
                image.Save(Path.Combine(ImagePath, id, $"{id}_medium.png"));

                image.Mutate(x => x.Resize(resolutionSize / 10, resolutionSize / 10));
                image.Save(Path.Combine(ImagePath, id, $"{id}_small.png"));
            }

            return id;
        }
    }
}
