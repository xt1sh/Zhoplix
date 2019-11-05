using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Episode
{
    public class CreateEpisodeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SeasonId { get; set; }
        public TimeSpan? OpeningStart { get; set; }
        public TimeSpan? OpeningFinish { get; set; }
        public TimeSpan? CreditsStart { get; set; }
        public IList<string> VideoPaths { get; set; }
    }
}
