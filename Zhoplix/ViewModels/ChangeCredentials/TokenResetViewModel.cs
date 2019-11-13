using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.ChangeCredentials
{
    public class TokenResetViewModel
    {
        public string Token { get; set; }

        public string Password { get; set; }

        public bool SignOutOfAll { get; set; }

        public string UserId { get; set; }

        public string Fingerprint { get; set; }

    }
}
