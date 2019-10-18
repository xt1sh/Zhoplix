using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;
using Zhoplix.Services.AuthenticationService.Response;

namespace Zhoplix.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<(bool, AccessTokenResponse)> AuthenticateAsync(User user, string password, bool rememberMe);

        Task<bool> CreateUserAsync(User user, string password);

        Task<(bool, DefaultResponse)> ConfirmUser(User user, string token, string role);
    }
}
