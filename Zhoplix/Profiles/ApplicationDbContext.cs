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
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EpisodeVideo> EpisodeVideos { get; set; }
        public DbSet<MovieVideo> MovieVideos { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ProfileTitle> ProfileTitle { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProfileEpisode>()
                .HasKey(bc => new { bc.ProfileId, bc.EpisodeId });
            builder.Entity<ProfileEpisode>()
                .HasOne(bc => bc.Profile)
                .WithMany(b => b.ProfileEpisodes)
                .HasForeignKey(bc => bc.ProfileId);
            builder.Entity<ProfileEpisode>()
                .HasOne(bc => bc.Episode)
                .WithMany(c => c.ProfileEpisode)
                .HasForeignKey(bc => bc.EpisodeId);

            builder.Entity<ProfileTitle>()
                .HasKey(k => new { k.ProfileId, k.TitleId });

            builder.Entity<TitleGenre>()
                .HasKey(k => new { k.TitleId, k.GenreId });

            builder.Entity<Rating>()
                .HasKey(k => new { k.ProfileId, k.TitleId });

            builder.Entity<Genre>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<Movie>()
                .HasKey(x => x.TitleId);
        }
    }
}