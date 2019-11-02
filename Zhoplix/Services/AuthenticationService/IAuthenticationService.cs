using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Zhoplix.Models.Identity;
using Zhoplix.Services.AuthenticationService.Response;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Authentication;

namespace Zhoplix.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<AccessTokenResponse> AuthenticateAsync(LoginViewModel model);

        Task<IEnumerable<IdentityError>> SignUpUserAsync(RegistrationViewModel model);

        Task<DefaultResponse> ConfirmUserAsync(EmailConfirmationViewModel model);

        Task<DefaultResponse> RefreshTokensAsync(RefreshViewModel model);

        Task<bool> SignOutAsync(string username, string fingerprint);
    }
}
