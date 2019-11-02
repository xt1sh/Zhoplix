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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Zhoplix.Models.Identity;

namespace Zhoplix.Services.TokenHandler
{
    
    public class TokenHandler : ITokenHandler
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public TokenHandler(IOptions<JwtConfiguration> jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public Task<string> GenerateAccessTokenAsync(User user, IList<Claim> claims)
        {

            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "access")
            };
            authClaims.AddRange(claims);
            
            var token = new JwtSecurityToken(
                claims: authClaims,
                expires: DateTime.Now.AddSeconds(_jwtConfiguration.AccessExpirationTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );
            
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<string> GenerateRefreshTokenAsync(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "refresh")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(_jwtConfiguration.RefreshExpirationTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}
