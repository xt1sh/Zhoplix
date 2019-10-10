using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zhoplix.Configurations;
using Zhoplix.Models.Identity;
using Zhoplix.Services.AuthenticationService.Response;
using Zhoplix.Services.TokenHandler;

namespace Zhoplix.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;

        public readonly ITokenHandler _tokenHandler;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IRepository<User> _userRepository;

        public AuthenticationService(UserManager<User> userManager,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            IRepository<User> userRepository)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _jwtConfig = jwtConfig.Value;
            _userRepository = userRepository;
        }

        public async Task<DefaultResponse> AuthenticateAsync(User user, string password, bool rememberMe)
        {
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var authClaims = new[]
{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(new List<Claim>(authClaims), _userManager.GetRolesAsync(user).Result);

                if (rememberMe)
                {
                    var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(new List<Claim>(authClaims));
                    user.RefreshToken = refreshToken;
                    await _userRepository.ChangeObjectAsync(user);

                    return new DefaultResponse(true, accessToken, refreshToken, _jwtConfig.AccessExpirationTime);
                }

                return new DefaultResponse(true, accessToken, null, _jwtConfig.AccessExpirationTime);
            }

            return new DefaultResponse(false);
        }

        public async Task<DefaultResponse> CreateUserAsync(User user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);

                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(new List<Claim>(authClaims), _userManager.GetRolesAsync(user).Result);
                var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(new List<Claim>(authClaims));

                user.RefreshToken = refreshToken;
                await _userRepository.ChangeObjectAsync(user);

                return new DefaultResponse(true, accessToken, refreshToken, _jwtConfig.AccessExpirationTime);

            }
            return new DefaultResponse(false);
        }

    }
}
