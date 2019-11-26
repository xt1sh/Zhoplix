using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public bool ValidateIssuer { get; set; }

        public bool ValidateAudience { get; set; }

        public double AccessExpirationTime { get; set; }

        public double RefreshExpirationTime { get; set; }

        public bool ValidateLifetime { get; set; }


    }
}
