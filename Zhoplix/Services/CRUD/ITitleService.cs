using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
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
        Task<IEnumerable<Title>> GetTitlePageAsync(int page, int pageSize);
        Task<bool> UpdateTitleAsync(Title title);
    }

    public class TitleService : ITitleService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Title> _titleContext;
        private readonly DbSet<Genre> _genreContext;
        private readonly ILogger<TitleService> _logger;
        private readonly IMapper _mapper;

        public TitleService(ApplicationDbContext context,
            ILogger<TitleService> logger,
            IMapper mapper)
        {
            _context = context;
            _titleContext = _context.Titles;
            _genreContext = _context.Genres;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Title> CreateTitleFromCreateViewModelAsync(CreateTitleViewModel model)
        {
            var title = _mapper.Map<Title>(model);
            var titleGenres = new List<TitleGenre>();

            foreach (var genrevm in model.Genres)
            {
                var genre = _mapper.Map<Genre>(genrevm);
                var genreFromContext = await _genreContext.FirstOrDefaultAsync(x => x.Name == genre.Name);
                if (genreFromContext == null)
                    genreFromContext = genre;

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

        public async Task<IEnumerable<Title>> GetTitlePageAsync(int page, int pageSize) =>
            await _titleContext.Skip(page * (pageSize - 1)).Take(pageSize).ToListAsync();

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
