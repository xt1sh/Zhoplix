using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Jobs
{
    [DisallowConcurrentExecution]
    public class RemoveExpiredSessions : IJob
    {
        private readonly IServiceProvider _provider;
        public RemoveExpiredSessions(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var _sessionContext = dbContext.Sessions;
                var sessions = _sessionContext.Where(s => s.ExpiresAt < DateTime.Now).ToList();
                _sessionContext.RemoveRange(sessions);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
