using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Zhoplix.Configurations;

namespace Zhoplix.Services.TokenHandler
{
    
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _config;
        private readonly JwtConfiguration _jwtConfiguration;

        public TokenHandler(IConfiguration config, IOptions<JwtConfiguration> jwtConfiguration)
        {
            _config = config;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public Task<string> GenerateAccessTokenAsync(List<Claim> claims)
        {

            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(_jwtConfiguration.AccessExpirationTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<string> GenerateRefreshTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
