using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.ViewModels.Episode;

namespace Zhoplix.Services.CRUD
{
    public class EpisodeService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Episode> _episodeContext;
        private readonly SeasonService _seasonService;
        private readonly IMapper _mapper;
        private readonly ILogger<EpisodeService> _logger;

        public EpisodeService(ApplicationDbContext context,
            SeasonService seasonService,
            IMapper mapper,
            ILogger<EpisodeService> logger)
        {
            _context = context;
            _episodeContext = _context.Episodes;
            _seasonService = seasonService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Episode> CreateEpisodeFromCreateViewModelAsync(CreateEpisodeViewModel model)
        {
            

            return null;
        }

        public async Task<bool> CreateEpisodeAsync(Episode episode)
        {
            _episodeContext.Add(episode);
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Episode>> GetAllEpisodesAsync() =>
            await _context.Episodes.ToListAsync();

        public async Task<Episode> GetEpisodeAsync(int id) =>
            await _episodeContext.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Episode> GetEpisodeAsync(Episode episode) =>
            await _episodeContext.FirstOrDefaultAsync(x => x == episode);

        public async Task<IEnumerable<Episode>> GetEpisodePageAsync(int page, int pageSize) =>
            await _episodeContext.Skip(page * (pageSize - 1)).Take(pageSize).ToListAsync();

        public async Task<bool> UpdateEpisodeAsync(Episode episode)
        {
            _episodeContext.Update(episode);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteEpisodeAsync(int id)
        {
            _episodeContext.Remove(await GetEpisodeAsync(id));
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteEpisodeAsync(Episode episode)
        {
            _episodeContext.Remove(episode);
            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
