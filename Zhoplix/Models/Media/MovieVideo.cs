using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Media;

namespace Zhoplix.Models
{
    public class MovieVideo
    {
        public string Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int VideoInfoId { get; set; }
        public VideoInfo VideoInfo { get; set; }
        public string Location { get; set; }
        [NotMapped]
        public string Resolution => $"{Width}x{Height}";
        public int Width { get; set; }
        public int Height { get; set; }
        public long Size { get; set; }
        [NotMapped]
        public float SizeInMb => Size / 1048576;
    }
}
