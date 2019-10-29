using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public IList<UserTitle> UserTitles { get; set; }
        public IList<UserEpisode> UserEpisodes { get; set; }
        public string RefreshToken { get; set; }
        public IList<Rating> Ratings { get; set; }
    }
}
