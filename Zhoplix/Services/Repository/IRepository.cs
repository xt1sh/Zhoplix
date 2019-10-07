using System.Threading.Tasks;

namespace Zhoplix.Services
{
    public interface IRepository<T>
    {
        Task<T> GetObjectByIdAsync(int id);
        Task AddObjectAsync(object model);
        Task<bool> SaveAllAsync();
    }
}