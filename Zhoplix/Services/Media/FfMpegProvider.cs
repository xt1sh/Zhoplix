﻿using Microsoft.Extensions.Logging;
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
        string CreateThumbnails(string filePath, string newPath);
        string ResizeVideo(string filePath, int width, int height = -1);
    }

    public class FfMpegProvider : IFfMpegProvider
    {
        public string CreateThumbnails(string filePath, string newPath)
        {
            Directory.CreateDirectory(newPath);
            var durationInSecs = double.Parse(ProcessCommand($"ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {filePath}"));
            var secondsForThumbnail = (int) Math.Log10(durationInSecs);
            var command = $"ffmpeg -i {filePath} -f image2 -bt 20M -vf fps=1/{secondsForThumbnail} {newPath}/%d.png";
            return ProcessCommand(command);
        }

        public string ConvertToMp4(string filePath)
        {
            var newPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
            return ProcessCommand($"ffmpeg -i {filePath} -c copy {newPath}.mp4");
        }

        public string ResizeVideo(string filePath, int width, int height = -1)
        {
            var newPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
            var command = $"ffmpeg -i {filePath} -filter:v scale={width}:{height} -c:a copy {newPath}_{width}.mp4";
            return ProcessCommand(command);
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