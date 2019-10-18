using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUrlHelper _url;
        private readonly IEmailSender _emailSender;


        public AuthenticationService(UserManager<User> userManager,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            IRepository<User> userRepository,
            IUrlHelper url,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _jwtConfig = jwtConfig.Value;
            _userRepository = userRepository;
            _url = url;
            _emailSender = emailSender;
        }

        public async Task<(bool, AccessTokenResponse)> AuthenticateAsync(User user, string password, bool rememberMe)
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

                    return (true, new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime));
                }

                return (true, new AccessTokenResponse(accessToken, _jwtConfig.AccessExpirationTime));
            }

            return (false, null);
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = _url.Action(
                    "confirmEmail",
                    "account",
                    new
                    {
                        userId = user.Id, token = emailConfirmationToken
                    },
                    protocol: "http"
                );
                var htmlMessage = $"<a href='{callbackUrl}'>link</a>";

                await _emailSender.SendEmailAsync(user.Email, "Account confirmation", htmlMessage);

                return true;
            }
            return false;
        }

        public async Task<(bool, DefaultResponse)> ConfirmUser(User user, string token, string role)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");

                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(new List<Claim>(authClaims),
                    _userManager.GetRolesAsync(user).Result);
                var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(new List<Claim>(authClaims));

                user.RefreshToken = refreshToken;
                await _userRepository.ChangeObjectAsync(user);


                return (true, new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime));

            }

            return (false, null);
        }

    }
}
