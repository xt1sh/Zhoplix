using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public List<UserTitle> UserTitles { get; set; }
        public List<UserEpisode> UserEpisodes { get; set; }
        public string RefreshToken { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
