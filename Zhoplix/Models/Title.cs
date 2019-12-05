using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Title
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Season> Seasons { get; set; }
        public IList<ProfileTitle> ProfileTitle { get; set; }
        public IList<TitleGenre> Genres { get; set; }
        public string Description { get; set; }
        [Range(0, 18)]
        public int AgeRestriction { get; set; }
        public string ImageId { get; set; }
        public string ImageLocation { get; set; }
        public IList<Rating> Ratings { get; set; }
        public bool IsMovie => Movie != null;
        public Movie Movie { get; set; }
    }
}
