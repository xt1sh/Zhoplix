using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.ViewModels.Episode;

namespace Zhoplix.ViewModels.Season
{
    public class SeasonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EpisodeViewModel> Episodes { get; set; }
        public string ImageLocation { get; set; }
    }
}
