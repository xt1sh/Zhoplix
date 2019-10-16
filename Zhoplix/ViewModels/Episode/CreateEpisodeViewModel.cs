using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Episode
{
    public class CreateEpisodeViewModel
    {
        public string Name { get; set; }
        public int SeasonId { get; set; }
        public string VideoId { get; set; }
    }
}
