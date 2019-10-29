using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Media;

namespace Zhoplix.Models
{
    public class Audio
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string Translation { get; set; }
        public int VideoInfoId { get; set; }
        public VideoInfo VideoInfo { get; set; }
    }
}
