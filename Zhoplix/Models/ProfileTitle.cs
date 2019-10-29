using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Models
{
    public class ProfileTitle
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }
    }
}
