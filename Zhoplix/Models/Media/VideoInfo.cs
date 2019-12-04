using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Media
{
    public class VideoInfo
    {
        public int Id { get; set; }
        public IList<EpisodeVideo> EpisodeVideos { get; set; }
        public IList<MovieVideo> MovieVideos { get; set; }
        public TimeSpan Duration { get; set; }
        public IList<Subtitles> Subtitles { get; set; }
        public IList<Audio> Audios { get; set; }
        public string Ration => $"{WidthRatio}:{HeightRatio}";
        public float WidthRatio { get; set; }
        public float HeightRatio { get; set; }
        public string Codec { get; set; }
    }
}
