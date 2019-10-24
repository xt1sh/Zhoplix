using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Models
{
    public class UserEpisode
    {
        public int UserId { get; set; }
        public int EpisodeId { get; set; }
        public User User { get; set; }
        public Episode Episode { get; set; }
        public TimeSpan TimeStopped { get; set; }
        public bool Finished { get; set; }
    }
}
