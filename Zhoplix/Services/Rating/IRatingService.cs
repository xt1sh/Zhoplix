using System.Threading.Tasks;

namespace Zhoplix.Services.Rating
{
    public interface IRatingService
    {
        Task RateTitle(int titleId, bool score);
    }
}