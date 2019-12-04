using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.Models.Identity;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Title;

namespace Zhoplix.Services.CRUD
{
    public interface ITitleService
    {
        Task<bool> CreateTitleAsync(Title title);
        Task<Title> CreateTitleFromCreateViewModelAsync(CreateTitleViewModel model);
        Task<bool> DeleteTitleAsync(int id);
        Task<bool> DeleteTitleAsync(Title title);
        Task<IEnumerable<Title>> GetAllTitlesAsync();
        Task<Title> GetTitleAsync(int id);
        Task<Title> GetTitleAsync(Title title);
        Task<Title> GetTitleByNameAsync(string titleName);
        Task<IList<Title>> FindTitlesAsync(string name);
        Task<IEnumerable<Title>> GetTitlePageAsync(int pageNumber, int pageSize);
        Task<bool> UpdateTitleAsync(Title title);
    }

    public class TitleService : ITitleService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly DbSet<Title> _titleContext;
        private readonly DbSet<Genre> _genreContext;
        private readonly ILogger<TitleService> _logger;
        private readonly IMapper _mapper;

        public TitleService(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<TitleService> logger,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _titleContext = _context.Titles;
            _genreContext = _context.Genres;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Title> CreateTitleFromCreateViewModelAsync(CreateTitleViewModel model)
        {
            var title = _mapper.Map<Title>(model);
            var titleGenres = new List<TitleGenre>();

            foreach (var genre in model.Genres)
            {
                var genreFromContext = await _genreContext.FirstOrDefaultAsync(x => x.Name == genre);
                if (genreFromContext is null)
                    genreFromContext = new Genre { Name = genre };

                titleGenres.Add(new TitleGenre
                {
                    Title = title,
                    Genre = genreFromContext,
                });
            }
            title.Genres = titleGenres;

            if (await CreateTitleAsync(title))
                return title;

            _logger.LogError("Failed to create title " + title.Name);
            return null;
        }

        public async Task<bool> CreateTitleAsync(Title title)
        {
            _titleContext.Add(title);
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Title>> GetAllTitlesAsync() =>
            await _context.Titles.ToListAsync();

        public async Task<Title> GetTitleAsync(int id) =>
            await _titleContext.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Title> GetTitleAsync(Title title) =>
            await _titleContext.FirstOrDefaultAsync(x => x == title);

        public async Task<Title> GetTitleByNameAsync(string titleName) =>
            await _titleContext.FirstOrDefaultAsync(x => x.Name == titleName);

        public async Task<IList<Title>> FindTitlesAsync(string name) =>
            await _titleContext.Where(x => EF.Functions.Like(x.Name, $"%{name}%")).Take(20).ToListAsync();

        public async Task<IEnumerable<Title>> GetTitlePageAsync(int pageNumber, int pageSize) =>
            await _titleContext.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

        public async Task<IList<Title>> GetMyList(string username, int pageNumber, int pageSize)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return null;
            return await _titleContext
                .Where(t => t.ProfileTitles.Any(pt => pt.ProfileId == user.Id))
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> UpdateTitleAsync(Title title)
        {
            _titleContext.Update(title);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteTitleAsync(int id)
        {
            _titleContext.Remove(await GetTitleAsync(id));
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteTitleAsync(Title title)
        {
            _titleContext.Remove(title);
            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
