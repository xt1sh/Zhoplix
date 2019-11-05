using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Services.Media
{
    public interface IFfMpegProvider
    {
        string ConvertToMp4(string filePath);
        string CreateThumbnails(string filePath);
        /// <summary>
        /// Creates resized copy of video
        /// </summary>
        /// <param name="filePath">Path to origin video file</param>
        /// <param name="width">New video width in pixels</param>
        /// <param name="height">Optional. New video height in pixels. If default 
        ///                      aspect ratio will be the same as origin</param>
        /// <returns>Full path of new video</returns>
        string ResizeVideo(string filePath, int width, int height = -1);
    }

    public class FfMpegProvider : IFfMpegProvider
    {
        public string CreateThumbnails(string filePath)
        {
            Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(filePath), "Thumbnails"));
            var durationInSecs = double.Parse(ProcessCommand($"ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {filePath}"));
            var secondsForThumbnail = (int) Math.Log10(durationInSecs);
            var command = $"ffmpeg -i {filePath} -f image2 -bt 20M -vf fps=1/{secondsForThumbnail} {Path.GetDirectoryName(filePath)}/Thumbnails/%d.png";
            return ProcessCommand(command);
        }

        public string ConvertToMp4(string filePath)
        {
            var newPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
            return ProcessCommand($"ffmpeg -i {filePath} -c copy {newPath}.mp4");
        }

        public string ResizeVideo(string filePath, int width, int height = -1)
        {
            var newPath = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}_{width}.mp4");

            var command = $"ffmpeg -i {filePath} -filter:v scale={width}:{height} -c:a copy {newPath}";
            ProcessCommand(command);
            return newPath;
        }

        private string ProcessCommand(string command)
        {
            using var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + command
            };
            process.StartInfo = startInfo;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
    }
}
