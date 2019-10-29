using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Media
{
    public class VideoInfo
    {
        public int Id { get; set; }
        public List<Video> Videos { get; set; }
        public TimeSpan Duration { get; set; }
        public IList<Subtitles> Subtitles { get; set; }
        public IList<Audio> Audios { get; set; }
        public string Ration => $"{WidthRatio}:{HeightRatio}";
        public float WidthRatio { get; set; }
        public float HeightRatio { get; set; }
        public string Codec { get; set; }
    }
}
