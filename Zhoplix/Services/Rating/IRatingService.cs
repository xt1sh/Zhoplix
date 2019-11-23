using System.Threading.Tasks;

namespace Zhoplix.Services.Rating
{
    public interface IRatingService
    {
        Task<bool> RateTitleAsync(int titleId, bool score);
        /// <summary>
        /// Percent of positive scores
        /// </summary>
        /// <returns>0-100%</returns>
        Task<float> GetTitleScoreAsync(int titleId);
    }
}