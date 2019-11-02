using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Authentication;
using Microsoft.Extensions.Logging;
using Zhoplix.Services.AuthenticationService;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Zhoplix.Services.ProfileManager;


namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;
        private readonly IProfileManager _profileManager;

        public AuthenticationController(IAuthenticationService authentication, IProfileManager profileManager)
        {
            _authentication = authentication;
            _profileManager = profileManager;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            
            var errors = await _authentication.SignUpUserAsync(model);

            if (errors is null)
            {
                return Ok();
            }

            return BadRequest(new
            {
                errors = errors
            });

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var response = await _authentication.AuthenticateAsync(model);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationViewModel model)
        {
            var (response, userId) = await _authentication.ConfirmUserAsync(model);

            if (response != null && userId != -1)
            {
                var profile = await _profileManager.CreateProfileAsync(userId);
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RefreshTokens(RefreshViewModel model)
        {
            var response = await _authentication.RefreshTokensAsync(model);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignOut(IDictionary<string, string> data)
        {

            var result = await _authentication.SignOutAsync(
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                data["fingerprint"]
                );

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
 
}