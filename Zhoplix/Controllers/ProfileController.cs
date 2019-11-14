using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zhoplix.Models.Identity;
using Zhoplix.Services.ProfileManager;
using Zhoplix.Services.RecoveryService;
using Zhoplix.ViewModels.Authentication;
using Zhoplix.ViewModels.ChangeCredentials;

namespace Zhoplix.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IProfileManager _profileManger;
        private readonly IRecoveryService _recovery;

        public ProfileController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IProfileManager profileManager,
            IRecoveryService recovery
            )
        {
            _context = context;
            _userManager = userManager;
            _profileManger = profileManager;
            _recovery = recovery;
        }

        [HttpGet]
        public async Task<IActionResult> YourAccount()
        {

            var user = await _userManager.FindByNameAsync(
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (user != null)
            {
                return Ok(new { 
                    username = user.UserName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber
                });
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordWithToken(TokenResetViewModel model)
        {
            var result = await _recovery.ChangePasswordWithToken(model);

            if (result is null)
                return Ok();

            return BadRequest(result);
        }

    }
}