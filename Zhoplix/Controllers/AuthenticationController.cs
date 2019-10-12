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
using System.Text.RegularExpressions;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
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
            _authentication = authentication;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var (isSuccess, response) = await _authentication.CreateUserAsync(user, model.Password, "Member");

            if (isSuccess)
            {
                return Ok(response);
            }

            return BadRequest();

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            var user = regex.IsMatch(model.Login)
                    ? await _userManager.FindByEmailAsync(model.Login)
                    : await _userManager.FindByNameAsync(model.Login);

            var (isSuccess, response) = await _authentication.AuthenticateAsync(user, model.Password, model.RememberMe);

            if (isSuccess)
            {

                if (model.RememberMe)
                    return Ok(response);

                return Ok(response);
            }

            return BadRequest();
        }
    }
 
}