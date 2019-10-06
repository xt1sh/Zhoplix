using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zhoplix.Models;
using Zhoplix.Models.Identity;

namespace Zhoplix
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Title> Titles { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEpisode>()
                .HasKey(bc => new { bc.UserId, bc.EpisodeId });
            builder.Entity<UserEpisode>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserEpisodes)
                .HasForeignKey(bc => bc.UserId);
            builder.Entity<UserEpisode>()
                .HasOne(bc => bc.Episode)
                .WithMany(c => c.UserEpisodes)
                .HasForeignKey(bc => bc.EpisodeId);
        }
    }
}