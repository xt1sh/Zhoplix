using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Movie
{
    public class CreateMovieViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<string> Genres { get; set; }
        public int AgeRestriction { get; set; }
        public string ImageLocation { get; set; }
        public TimeSpan? CreditsStart { get; set; }
        public IList<string> VideoPaths { get; set; }
    }
}
