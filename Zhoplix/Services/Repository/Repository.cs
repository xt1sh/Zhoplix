using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;

namespace Zhoplix.Services
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Repository<T>> _logger;

        public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> GetObjectByIdAsync(int id) =>
            await _context.FindAsync<T>(id);

        public async Task AddObjectAsync(T model)
        {
            await _context.AddAsync(model);
            await SaveAllAsync();
        }

        private async Task SaveAllAsync()
        {
            if (await _context.SaveChangesAsync() == 0)
                _logger.LogError($"Error while saving db");
        }
    }
}
