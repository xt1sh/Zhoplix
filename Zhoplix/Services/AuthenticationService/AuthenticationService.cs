using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Zhoplix.Configurations;
using Zhoplix.Models.Identity;
using Zhoplix.Services.AuthenticationService.Response;
using Zhoplix.Services.TokenHandler;
using Microsoft.EntityFrameworkCore;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Authentication;

namespace Zhoplix.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;

        public readonly ITokenHandler _tokenHandler;
        private readonly JwtConfiguration _jwtConfig;
        private readonly ApplicationDbContext _context;
        private readonly IUrlHelper _url;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly DbSet<Session> _sessionContext;


        public AuthenticationService(
            UserManager<User> userManager,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            ApplicationDbContext context,
            IUrlHelper url,
            IEmailSender emailSender,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _jwtConfig = jwtConfig.Value;
            _context = context;
            _sessionContext = context.Sessions;
            _url = url;
            _emailSender = emailSender;
            _mapper = mapper;

        }

        public async Task<AccessTokenResponse> AuthenticateAsync(LoginViewModel model)
        {
            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            var user = regex.IsMatch(model.Login)
                ? await _userManager.FindByEmailAsync(model.Login)
                : await _userManager.FindByNameAsync(model.Login);


            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new[]
{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(new List<Claim>(authClaims), _userManager.GetRolesAsync(user).Result);

                if (model.RememberMe)
                {
                    var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(new List<Claim>(authClaims));
                    _sessionContext.Add(new Session
                    {
                        User = user,
                        RefreshToken = refreshToken,
                        Fingerprint = model.Fingerprint,
                        CreatedAt = DateTime.Now
                    });

                    if (await _context.SaveChangesAsync() > 0)
                        return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);
                }
                else
                {
                    return new AccessTokenResponse(accessToken, _jwtConfig.AccessExpirationTime);
                }
            }

            return null;
        }

        public async Task<IEnumerable<IdentityError>> CreateUserAsync(RegistrationViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password); 
            if (result.Succeeded)   
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var htmlMessage = this.GenerateConfirmationMessage(user.Id, emailConfirmationToken);

                await _emailSender.SendEmailAsync(user.Email, "Account confirmation", htmlMessage);

                return null;
            }
            return result.Errors;
        }

        public async Task<DefaultResponse> ConfirmUser(EmailConfirmationViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.Token))
                return null;

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
                return null;

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
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

                _sessionContext.Add(new Session
                {
                    User = user,
                    RefreshToken = refreshToken,
                    Fingerprint = model.Fingerprint,
                    CreatedAt = DateTime.Now
                });

                if (await _context.SaveChangesAsync() > 0)
                    return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);
            }

            return null;
        }

        public string GenerateConfirmationMessage(int userId, string token)
        {
            var callbackUrl = _url.Action(
                "confirmEmail",
                "account",
                new
                {
                    userId = userId,
                    token = token
                },
                protocol: "http"
            );
             return $"<a href='{callbackUrl}'>link</a>";
        }


    }
}
