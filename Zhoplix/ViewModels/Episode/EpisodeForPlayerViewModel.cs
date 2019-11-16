using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.ViewModels.Media;

namespace Zhoplix.ViewModels.Episode
{
    public class EpisodeForPlayerViewModel
    {
        public int ThumbnailsAmount { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ThumbnailLocation { get; set; }
        [NotMapped]
        public List<VideoForPlayerViewModel> Videos { get; set; }
    }
}
