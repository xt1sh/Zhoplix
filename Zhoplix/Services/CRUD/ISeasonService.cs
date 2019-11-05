using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.ViewModels.Season;

namespace Zhoplix.Services.CRUD
{
    public interface ISeasonService
    {
        Task<bool> CreateSeasonAsync(Season season);
        Task<Season> CreateSeasonFromCreateViewModelAsync(CreateSeasonViewModel model);
        Task<bool> DeleteSeasonAsync(int id);
        Task<bool> DeleteSeasonAsync(Season season);
        Task<IEnumerable<Season>> GetAllSeasonsOfTitleAsync(int titleId);
        Task<Season> GetSeasonAsync(int id);
        Task<Season> GetSeasonAsync(Season season);
        Task<IEnumerable<Season>> GetSeasonPageAsync(int page, int pageSize);
        Task<bool> UpdateSeasonAsync(Season season);
    }

    public class SeasonService : ISeasonService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITitleService _titleService;
        private readonly DbSet<Season> _seasonContext;
        private readonly IMapper _mapper;

        public SeasonService(ApplicationDbContext context,
            ITitleService titleService,
            IMapper mapper)
        {
            _context = context;
            _titleService = titleService;
            _seasonContext = _context.Seasons;
            _mapper = mapper;
        }

        public async Task<Season> CreateSeasonFromCreateViewModelAsync(CreateSeasonViewModel model)
        {
            var season = _mapper.Map<Season>(model);
            var title = await _titleService.GetTitleAsync(model.TitleId);

            if (title == null)
                return null;

            season.Title = title;

            var existSeason = _seasonContext.FirstOrDefault(x => x.TitleId == season.TitleId && x.Name == season.Name);

            if (existSeason != null)
                return null;

            if (await CreateSeasonAsync(season))
                return season;

            return null;
        }

        public async Task<bool> CreateSeasonAsync(Season season)
        {
            _seasonContext.Add(season);
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Season>> GetAllSeasonsOfTitleAsync(int titleId) =>
            await _context.Seasons.Where(x => x.TitleId == titleId).ToListAsync();

        public async Task<Season> GetSeasonAsync(int id) =>
            await _seasonContext.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Season> GetSeasonAsync(Season season) =>
            await _seasonContext.FirstOrDefaultAsync(x => x == season);

        public async Task<IEnumerable<Season>> GetSeasonPageAsync(int page, int pageSize) =>
            await _seasonContext.Skip(page * (pageSize - 1)).Take(pageSize).ToListAsync();

        public async Task<bool> UpdateSeasonAsync(Season season)
        {
            _seasonContext.Update(season);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteSeasonAsync(int id)
        {
            _seasonContext.Remove(await GetSeasonAsync(id));
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteSeasonAsync(Season season)
        {
            _seasonContext.Remove(season);
            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
