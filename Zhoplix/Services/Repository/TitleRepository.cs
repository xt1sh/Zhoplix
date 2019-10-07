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

        public async Task AddObjectAsync(object model)
        {
            try
            {
                await _context.AddAsync(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while creating entity {model}: {e.Message}");
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }
    }
}
