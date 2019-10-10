using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Zhoplix.Configurations;
using Zhoplix.Models.Identity;
using Zhoplix.Services.TokenHandler;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Authentication;
using Zhoplix.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Zhoplix.Services.AuthenticationService;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(UserManager<User> userManager,
            IMapper mapper,
            ITokenHandler tokenHandler,
            IOptions<JwtConfiguration> jwtConfig,
            IRepository<User> userRepository,
            IAuthenticationService authentication,
            ILogger<AuthenticationController> logger
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _jwtConfig = jwtConfig.Value;
            _userRepository = userRepository;
            _authentication = authentication;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            var user = _mapper.Map<RegistrationViewModel, User>(model);
            var response = await _authentication.CreateUserAsync(user, model.Password, "Member");

            if (response.Success)
            { 
                return Ok(new
                {
                    accessToken = response.AccessToken,
                    refreshToken = response.RefreshToken,
                    expirationTime = response.ExpirationTime
                });
            }

            return BadRequest();

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login) ?? await _userManager.FindByEmailAsync(model.Login);
            var response = await _authentication.AuthenticateAsync(user, model.Password, model.RememberMe);

            if (response.Success)
            { 

                if (model.RememberMe)
                    return Ok(new
                    {
                        accessToken = response.AccessToken,
                        refreshToken = response.RefreshToken,
                        expirationTime = response.ExpirationTime
                    });

                return Ok(new
                {
                    accessToken = response.AccessToken,
                    expirationTime = response.ExpirationTime
                });
            }

            return BadRequest();
        }
        
        
    }
    
}