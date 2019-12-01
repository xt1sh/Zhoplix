using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Movie
    {
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public IList<Video> Videos { get; set; }
        public string Location { get; set; }
        public int ThumbnailsAmount { get; set; }
        public TimeSpan? CreditsStart { get; set; }
    }
}
