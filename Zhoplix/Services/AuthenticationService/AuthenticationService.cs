using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Zhoplix.Configurations;
using Zhoplix.Models.Identity;
using Zhoplix.Services.AuthenticationService.Response;
using Zhoplix.Services.TokenHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zhoplix.Services.ProfileManager;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Authentication;
using Profile = AutoMapper.Profile;

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
        private readonly ILogger _logger;
        private readonly DbSet<Session> _sessionContext;
        private readonly IProfileManager _profileManager;


        public AuthenticationService(
            UserManager<User> userManager,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            IProfileManager profileManager,
            ApplicationDbContext context,
            IUrlHelper url,
            IEmailSender emailSender,
            ILogger<AuthenticationService> logger,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _profileManager = profileManager;
            _jwtConfig = jwtConfig.Value;
            _context = context;
            _sessionContext = _context.Sessions;
            _url = url;
            _emailSender = emailSender;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<AccessTokenResponse> AuthenticateAsync(LoginViewModel model)
        {

            if (model.Login is null)
                return null;

            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            
            var user = regex.IsMatch(model.Login)
                ? await _userManager.FindByEmailAsync(model.Login)
                : await _userManager.FindByNameAsync(model.Login);


            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return null;

            var session = await _sessionContext.FirstOrDefaultAsync(s => 
                (s.UserId == user.Id) && (s.Fingerprint == model.Fingerprint)
                );

            var accessToken = await GenerateAccessWithClaims(user);


            if (session != null)
            {
                _sessionContext.Remove(session);
                await _context.SaveChangesAsync();
            }

            if (model.RememberMe)
            {
                var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);
                _sessionContext.Add(new Session
                {
                    User = user,
                    RefreshToken = refreshToken,
                    Fingerprint = model.Fingerprint,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddSeconds(_jwtConfig.RefreshExpirationTime)
                });

                if (await _context.SaveChangesAsync() > 0)
                    return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);
            }
            else
            {
                return new AccessTokenResponse(accessToken, _jwtConfig.AccessExpirationTime);
            }
            
            return null;
        }

        public async Task<AccessTokenResponse> AuthenticateAsync(User user, string fingerprint)
        {
            var session = await _sessionContext.FirstOrDefaultAsync(s =>
                (s.UserId == user.Id) && (s.Fingerprint == fingerprint)
                );

            var accessToken = await GenerateAccessWithClaims(user);


            if (session != null)
            {
                _sessionContext.Remove(session);
                await _context.SaveChangesAsync();
            }

            var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);
            _sessionContext.Add(new Session
            {
                User = user,
                RefreshToken = refreshToken,
                Fingerprint = fingerprint,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddSeconds(_jwtConfig.RefreshExpirationTime)
            });

            if (await _context.SaveChangesAsync() > 0)
                return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);

            return null;
        }

        public async Task<IEnumerable<IdentityError>> SignUpUserAsync(RegistrationViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password); 
            if (result.Succeeded)   
            {
                _ = await _profileManager.CreateProfileAsync(user.Id);
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

                var accessToken = await GenerateAccessWithClaims(user);
                var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);

                _sessionContext.Add(new Session
                {
                    User = user,
                    RefreshToken = refreshToken,
                    Fingerprint = model.Fingerprint,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddSeconds(_jwtConfig.RefreshExpirationTime)
                });

                if (await _context.SaveChangesAsync() > 0)
                    return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);
            }

            return null;
        }

        public async Task<DefaultResponse> RefreshTokensAsync(RefreshViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RefreshToken) || string.IsNullOrWhiteSpace(model.Fingerprint))
                return null;

            var session = await _sessionContext.FirstOrDefaultAsync(s => s.RefreshToken == model.RefreshToken);

            if (session is null)
                return null;

            if (session.ExpiresAt < DateTime.Now || session.Fingerprint != model.Fingerprint)
            {
                _sessionContext.Remove(session);
                await _context.SaveChangesAsync();
                return null;
            }
            

            var user = await _userManager.FindByIdAsync(session.UserId.ToString());
            var accessToken = await GenerateAccessWithClaims(user);
            var refreshToken = await _tokenHandler.GenerateRefreshTokenAsync(user);

            session.RefreshToken = refreshToken;
            session.UpdatedAt = DateTime.Now;
            _sessionContext.Update(session);

            if (await _context.SaveChangesAsync() > 0)
                return new DefaultResponse(accessToken, refreshToken, _jwtConfig.AccessExpirationTime);

            return null;
        }

        public async Task<bool> SignOutAsync(string username, string fingerprint)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fingerprint))
                return false;

            var session = await _sessionContext.FirstOrDefaultAsync(s => (s.User.UserName == username) && (s.Fingerprint == fingerprint));
            if (session is null)
                return true;

            _sessionContext.Remove(session);
            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public async Task<bool> SignOutOfAllAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return false;

            _sessionContext.RemoveRange(_sessionContext.Where(s => s.UserId == user.Id).ToList());

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
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
             return $"<a href='{callbackUrl}'>Confirm Email</a>";
        }

        public async Task<string> GenerateAccessWithClaims(User user)
        {
            var profile = await _profileManager.GetProfileByIdAsync(user.Id);
            var claims = new List<Claim>
            {
                new Claim("avatar", profile.ImagePath)
            };
            claims.AddRange(from role in await _userManager.GetRolesAsync(user)
                select new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

            return await _tokenHandler.GenerateAccessTokenAsync(user, claims);
        }

        public async Task<bool> VerifySessionAsync(string username, string fingerprint)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return false;

            var session = await _sessionContext.FirstOrDefaultAsync(s => s.UserId == user.Id && s.Fingerprint == fingerprint);
            if (session is null)
                return false;

            return true;
        }
    }
}
