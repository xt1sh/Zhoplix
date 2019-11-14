using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Authentication
{
    public class ResetCodeViewModel
    {
        public string UserId { get; set; }

        public string Code { get; set; }

        public string Fingerprint { get; set; }
    }
}
