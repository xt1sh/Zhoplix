using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Season Season { get; set; }
        public List<UserEpisode> UserEpisodes { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
