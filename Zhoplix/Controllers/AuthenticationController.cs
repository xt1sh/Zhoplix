using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using Zhoplix.Services.AuthenticationService.Response;
using Microsoft.AspNetCore.Authorization;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;

        public AuthenticationController(IAuthenticationService authentication)
        {
            _authentication = authentication;
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
            var response = await _authentication.ConfirmUserAsync(model);

            if (response != null)
            {
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
    }
 
}