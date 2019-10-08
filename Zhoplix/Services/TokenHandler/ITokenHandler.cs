using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Zhoplix.Services.TokenHandler
{
    public interface ITokenHandler
    {
        Task<string> GenerateAccessTokenAsync(List<Claim> claims, IEnumerable<string> roles);
        Task<string> GenerateRefreshTokenAsync(List<Claim> claims);
    }
}
