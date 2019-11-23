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
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Models.Rating> _ratingContext;
        private readonly DbSet<Profile> _profileContext;
        private readonly ITitleService _titleService;
        private readonly UserManager<User> _userManager;
        private readonly HttpContext _httpContext;

        public RatingService(ApplicationDbContext context,
            ITitleService titleService,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _ratingContext = context.Ratings;
            _profileContext = context.Profiles;
            _titleService = titleService;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<bool> RateTitleAsync(int titleId, bool score)
        {
            var user = await _userManager.FindByNameAsync(
                _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var profile = await _profileContext.FirstOrDefaultAsync(x => x.User == user);
            var currentRating = await _ratingContext.FirstOrDefaultAsync(x => x.ProfileId == profile.Id && x.TitleId == titleId);

            if (currentRating != null)
            {
                if (currentRating.Liked == score)
                    _ratingContext.Remove(currentRating);
                else
                {
                    currentRating.Liked = score;
                    _ratingContext.Update(currentRating);
                }

                return await SaveCangesAsync();
            }

            var rating = new Models.Rating
            {
                ProfileId = profile.Id,
                TitleId = titleId,
                Liked = score
            };

            await _ratingContext.AddAsync(rating);
            return await SaveCangesAsync();
        }

        public async Task<float> GetTitleScoreAsync(int titleId)
        {
            var title = _ratingContext.Where(x => x.TitleId == titleId);
            var count = await title.CountAsync();
            var liked = await title.Where(x => x.Liked).CountAsync();
            return liked / count * 100;
        }

        private async Task<bool> SaveCangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
