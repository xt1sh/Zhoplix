using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public override int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
