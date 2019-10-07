using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Configurations
{
    public class PasswordConfiguration
    {
        public bool RequireNonAlphanumeric { get; set; }

        public bool RequireDigit { get; set; }

        public bool RequireLowercase { get; set; }

        public bool RequireUppercase { get; set; }

        public int RequiredLength { get; set; }

        public int RequiredUniqueChars { get; set; }

    }
}
