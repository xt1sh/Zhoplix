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
    public class SeasonService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Season> _seasonContext;
        private readonly IMapper _mapper;

        public SeasonService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _seasonContext = _context.Seasons;
            _mapper = mapper;
        }

        public async Task<Season> CreateSeasonFromCreateViewModelAsync(CreateSeasonViewModel model)
        {
            var season = _mapper.Map<Season>(model);

            return null;
        }

        public async Task<bool> CreateSeasonAsync(Season season)
        {
            _seasonContext.Add(season);
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Season>> GetAllSeasonsAsync() =>
            await _context.Seasons.ToListAsync();

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
