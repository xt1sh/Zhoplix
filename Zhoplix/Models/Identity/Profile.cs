using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Identity
{
    public class Profile
    {
        [ForeignKey("User")]
        public int Id { get; set; }
        public User User { get; set; }
        public string ImagePath { get; set; }
        public IList<ProfileTitle> ProfileTitles { get; set; }
        public IList<ProfileEpisode> ProfileEpisodes { get; set; }
        public IList<Rating> Ratings { get; set; }
    }
}
