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
using Org.BouncyCastle.Ocsp;
using Zhoplix.Services.AuthenticationService.Response;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthenticationController(UserManager<User> userManager,
  
            IAuthenticationService authentication
        )
        {

            _authentication = authentication;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            
            var errors = await _authentication.CreateUserAsync(model);

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


            var response = await _authentication.ConfirmUser(model);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest();
        }
    }
 
}