using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Models
{
    public class Rating
    {
        public int UserId { get; set; }
        public int TitleId { get; set; }
        public User User { get; set; }
        public Title Title { get; set; }
        public bool Liked { get; set; }
    }
}
