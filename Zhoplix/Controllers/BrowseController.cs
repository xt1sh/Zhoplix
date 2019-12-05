using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zhoplix.Services.CRUD;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BrowseController : ControllerBase
    {
        private readonly ITitleService _titleRepository;

        public BrowseController(ITitleService titleRepository)
        {
            _titleRepository = titleRepository;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetMyListPage(int pageNumber, int pageSize)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest();
            var titles = await _titleRepository.GetMyList(username, pageNumber, pageSize);

            if (titles is null)
                return BadRequest();

            return Ok(titles);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyListSize()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest();
            var titlesLenght = await _titleRepository.GetMyListSize();

            return Ok(new { length = titlesLenght });
        }
    }
}
