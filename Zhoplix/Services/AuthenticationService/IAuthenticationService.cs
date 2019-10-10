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
        Task<DefaultResponse> AuthenticateAsync(User user, string password, bool rememberMe);

        Task<DefaultResponse> CreateUserAsync(User user, string password, string role);
    }
}
