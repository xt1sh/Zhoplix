using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Authentication
{
    public class RefreshViewModel
    {
        public string RefreshToken { get; set; }
        public string Fingerprint { get; set; }
    }
}
