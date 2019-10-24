using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zhoplix.Models;
using Zhoplix.Models.Identity;

namespace Zhoplix
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Title> Titles { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Rating> Ratings { get; set; }

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

            builder.Entity<UserTitle>()
                .HasKey(k => new { k.UserId, k.TitleId });

            builder.Entity<TitleGenre>()
                .HasKey(k => new { k.TitleId, k.GenreId });

            builder.Entity<Rating>()
                .HasKey(k => new { k.UserId, k.TitleId });
        }
    }
}