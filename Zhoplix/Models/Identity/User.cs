using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Identity
{
    public class User : IdentityUser
    {
        public List<Title> TitlesWatching { get; set; }
        public List<UserEpisode> UserEpisodes { get; set; }
    }
}
