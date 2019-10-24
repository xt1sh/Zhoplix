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
        public List<Season> Seasons { get; set; }
        public List<UserTitle> UserTitles { get; set; }
        public List<TitleGenre> Genres { get; set; }
        public string Description { get; set; }
        [Range(0, 18)]
        public int AgeRestriction { get; set; }
        public string ImageId { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
