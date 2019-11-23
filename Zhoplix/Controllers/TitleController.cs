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
    [Route("[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly RatingService _ratingService;

        public TitleController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> RateTitle(PostRatingViewModel score)
        {
            return Ok();
        }
    }
}