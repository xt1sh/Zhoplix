using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;
using Zhoplix.Services.CRUD;

namespace Zhoplix.Services.Rating
{
    public class RatingService : IRatingService
    {
        private readonly DbSet<Models.Rating> _ratingContext;
        private readonly DbSet<Profile> _profileContext;
        private readonly ITitleService _titleService;
        private readonly UserManager<User> _userManager;
        private readonly HttpContext _httpContext;

        public RatingService(ApplicationDbContext context,
            ITitleService titleService,
            UserManager<User> userManager,
            HttpContext httpContext)
        {
            _ratingContext = context.Ratings;
            _profileContext = context.Profiles;
            _titleService = titleService;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task RateTitle(int titleId, bool score)
        {
            var user = await _userManager.FindByNameAsync(
                _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var title = await _titleService.GetTitleAsync(titleId);

            var rating = new Models.Rating
            {
                Profile = user.Profile,
                Title = title,
                Liked = score
            };

            _ratingContext.Add(rating);
        }
    }
}
