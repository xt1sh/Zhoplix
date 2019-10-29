using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Authentication
{
    public class EmailConfirmationViewModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Fingerprint { get; set; }
    }
}
