using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Models
{
    public class ProfileEpisode
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public TimeSpan TimeStopped { get; set; }
        public bool Finished { get; set; }
    }
}
