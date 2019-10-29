using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Services.TokenHandler
{
    public interface ITokenHandler
    {
        Task<string> GenerateAccessTokenAsync(User user, IEnumerable<string> roles);
        Task<string> GenerateRefreshTokenAsync(User user);
        DateTime ValidTo(string token);
    }
}
