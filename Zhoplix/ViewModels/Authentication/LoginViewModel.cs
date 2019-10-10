using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.ViewModels.Authentication
{
    public class LoginViewModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
