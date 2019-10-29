using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Media
{
    public class VideoInfo
    {
        public int Id { get; set; }
        public TimeSpan Duration { get; set; }
        public string Ration => $"{WidthRatio}:{HeightRatio}";
        public float WidthRatio { get; set; }
        public float HeightRatio { get; set; }
        public string Codec { get; set; }
    }
}
