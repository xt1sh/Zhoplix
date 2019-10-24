using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Video
    {
        public string Id { get; set; }
        public Episode Episode { get; set; }
        public string Location { get; set; }
        [NotMapped]
        public string Resolution => $"{Width}x{Height}";
        public int Width { get; set; }
        public int Height { get; set; }
        [NotMapped]
        public string Ration => $"{WidthRatio}:{HeightRatio}";
        public float WidthRatio { get; set; }
        public float HeightRatio { get; set; }
        public string Codec { get; set; }
        public long Size { get; set; }
        [NotMapped]
        public float SizeInMb => Size / 1048576;
    }
}
