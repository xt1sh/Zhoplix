using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Models
{
    public class UserTitle
    {
        public int UserId { get; set; }
        public int TitleId { get; set; }
        public User User { get; set; }
        public Title Title { get; set; }
    }
}
