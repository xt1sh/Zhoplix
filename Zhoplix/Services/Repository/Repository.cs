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

        public async Task<T> GetObjectByInstanceAsync(T obj) =>
            await _context.FindAsync<T>(obj);

        public async Task AddObjectAsync(T model)
        {
            await _context.AddAsync(model);
            await SaveAllAsync();
        }

        public async Task AddRangeAsync(T[] objs)
        {
            await _context.AddRangeAsync(objs);
        }

        public async Task ChangeObjectAsync(T newObj)
        {
            _context.Update(newObj);
            await SaveAllAsync();
        }

        public async Task ChangeRangeAsync(T[] newObjs)
        {
            _context.UpdateRange(newObjs);
            await SaveAllAsync();
        }

        public async Task ChangeObjectByIdAsync(int id, T newObj)
        {
            try
            {
                var obj = await GetObjectByIdAsync(id);
                newObj.GetType().GetProperty("Id").SetValue(obj, id);
                await ChangeObjectAsync(newObj);
            }
            catch(Exception e)
            {
                _logger.LogError($"ChangeObjectByIdAsync error: {e.Message}");
            }
        }

        public async Task DeleteObjectAsync(T obj)
        {
            _context.Remove(obj);
            await SaveAllAsync();
        }

        public async Task DeleteObjectByIdAsync(int id)
        {
            _context.Remove(await GetObjectByIdAsync(id));
            await SaveAllAsync();
        }

        public async Task DeleteRangeAsync(T[] objs)
        {
            _context.RemoveRange(objs);
            await SaveAllAsync();
        }

        public async Task DeleteRangeByIdsAsync(int[] ids)
        {
            var objectsToRemove = ids.Select(async id => await GetObjectByIdAsync(id));
            _context.RemoveRange(objectsToRemove);
            await SaveAllAsync();
        }

        private async Task SaveAllAsync()
        {
            if (await _context.SaveChangesAsync() == 0)
                _logger.LogError($"Error while saving db");
        }
    }
}
