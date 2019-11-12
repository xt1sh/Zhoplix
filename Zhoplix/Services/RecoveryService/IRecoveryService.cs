using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;
using Zhoplix.ViewModels.Authentication;

namespace Zhoplix.Services.RecoveryService
{
    public interface IRecoveryService
    {
        Task<bool> SendResetPasswordMessageAsync(string identifier);

        Task<User> VerifyPasswordResetCodeAsync(ResetCodeViewModel model);
    }
}
