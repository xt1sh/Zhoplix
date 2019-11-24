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
using Zhoplix.Services.RecoveryService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;
        private readonly IRecoveryService _recovery;

        public AuthenticationController(IAuthenticationService authentication,
                                        IRecoveryService recovery)
        {
            _authentication = authentication;
            _recovery = recovery;
        }

        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationViewModel model)
        {
            var response = await _authentication.ConfirmUserAsync(model);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SignOut(IDictionary<string, string> data)
        {
            if (!data.Keys.Contains("fingerprint"))
                return BadRequest();

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

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SignOutOfAll()
        {
            var result = await _authentication.SignOutOfAllAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(IDictionary<string, string> data)
        {
            if (!data.Keys.Contains("identifier"))
                return BadRequest();

            bool result = await _recovery.SendResetPasswordMessageAsync(data["identifier"]);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaswordResetCode(ResetCodeViewModel model)
        {
            var user = await _recovery.VerifyPasswordResetCodeAsync(model);

            if (user is null)
                return BadRequest();

            var response = await _authentication.AuthenticateAsync(user, model.Fingerprint);

            if (response != null)
                return Ok(response);

            return BadRequest();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifySession(IDictionary<string, string> data)
        {
            if (!data.Keys.Contains("fingerprint"))
                return BadRequest();

            bool result = await _authentication.VerifySessionAsync(
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                data["fingerprint"]
                );

            if (result)
            {
                return Ok();
            }

            return NotFound();

        }
    }
 
}