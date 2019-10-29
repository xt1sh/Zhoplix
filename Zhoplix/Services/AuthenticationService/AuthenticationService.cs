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
using Microsoft.Extensions.Logging;
using Zhoplix.Controllers;
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
        private readonly ILogger<AdminController> _logger;
        private readonly DbSet<Session> _sessionContext;


        public AuthenticationService(
            UserManager<User> userManager,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            ApplicationDbContext context,
            IUrlHelper url,
            IEmailSender emailSender,
            ILogger<AdminController> logger,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _jwtConfig = jwtConfig.Value;
            _context = context;
            _userContext = _context.Users;
            _url = url;
            _emailSender = emailSender;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<AccessTokenResponse> AuthenticateAsync(LoginViewModel model)
        {
            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            var user = regex.IsMatch(model.Login)
                ? await _userManager.FindByEmailAsync(model.Login)
                : await _userManager.FindByNameAsync(model.Login);


            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                

                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(user, _userManager.GetRolesAsync(user).Result);

                if (model.RememberMe)
                {
                    var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);
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

        public async Task<IEnumerable<IdentityError>> SignUpUserAsync(RegistrationViewModel model)
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

        public async Task<DefaultResponse> ConfirmUserAsync(EmailConfirmationViewModel model)
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

                var accessToken = await _tokenHandler.GenerateAccessTokenAsync(user, _userManager.GetRolesAsync(user).Result);
                var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);

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

        public async Task<DefaultResponse> RefreshTokensAsync(RefreshViewModel model)
        {
            var session = await _sessionContext.FirstOrDefaultAsync(s => s.RefreshToken == model.RefreshToken);
            if (DateTime.Now > _tokenHandler.ValidTo(model.RefreshToken) || session.Fingerprint != model.Fingerprint)
                return null;
            
            _sessionContext.Remove(session);
            var user = await _userManager.FindByIdAsync(session.UserId.ToString());
            var accessToken = await _tokenHandler.GenerateAccessTokenAsync(user, _userManager.GetRolesAsync(user).Result);
            var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);
            _sessionContext.Add(new Session
            {
                User = user,
                RefreshToken = model.RefreshToken,
                Fingerprint = model.Fingerprint,
                CreatedAt = DateTime.Now
            });

            if (await _context.SaveChangesAsync() > 0)
                return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);

            return null;
        }

        public async Task<DefaultResponse> RefreshTokensAsync(RefreshViewModel model)
        {
            var session = await _sessionContext.FirstOrDefaultAsync(s => s.RefreshToken == model.RefreshToken);
            if (DateTime.Now > _tokenHandler.ValidTo(model.RefreshToken) || session.Fingerprint != model.Fingerprint)
                return null;
            
            _sessionContext.Remove(session);
            var user = await _userManager.FindByIdAsync(session.UserId.ToString());
            var accessToken = await _tokenHandler.GenerateAccessTokenAsync(user, _userManager.GetRolesAsync(user).Result);
            var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);
            _sessionContext.Add(new Session
            {
                User = user,
                RefreshToken = model.RefreshToken,
                Fingerprint = model.Fingerprint,
                CreatedAt = DateTime.Now
            });

            if (await _context.SaveChangesAsync() > 0)
                return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);

            return null;
        }

        public string GenerateConfirmationMessage(int userId, string token)
        {
            var callbackUrl = _url.Action(
                "confirmEmail",
                "account",
                new
                {
                    userId,
                    token
                },
                protocol: "http"
            );
             return $"<a href='{callbackUrl}'>link</a>";
        }

    }
}
