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

namespace Zhoplix.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IProfileManager _profileManger;

        public ProfileController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IProfileManager profileManager
            )
        {
            _context = context;
            _userManager = userManager;
            _profileManger = profileManager;

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
    }
}