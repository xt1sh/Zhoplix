using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class TitleGenre
    {
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
