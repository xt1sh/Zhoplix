using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zhoplix.Models.Identity;
using Zhoplix.Services.Rating;
using Zhoplix.ViewModels.Rating;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public TitleController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> RateTitle(PostRatingViewModel score)
        {
            var result = await _ratingService.RateTitleAsync(score.TitleId, score.Score);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTitleScore(int id)
        {
            var result = await _ratingService.GetTitleScoreAsync(id);
            return Ok(result);
        }
    }
}