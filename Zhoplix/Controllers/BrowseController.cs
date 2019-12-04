using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BrowseController : ControllerBase
    {


        public BrowseController()
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetMyList()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest();
            //var titles = await _titleRepository.GetMyList(username);

            //if (titles is null)
            //    return BadRequest();

            //return Ok(titles);
            return Ok();
        }
    }
}
