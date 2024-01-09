using Microsoft.EntityFrameworkCore;
using MovieWeb.Models;
using System.Reflection.Emit;

namespace MovieWeb.Data
{
    public class MovieWebDBContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Nation> Nation { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<MovieUser> Follow { get; set; }
        public MovieWebDBContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<MovieGenre>().HasKey(mg => new {mg.MovieID, mg.GenreID});
            builder.Entity<MovieGenre>()
                    .HasOne(m => m.Movie)
                    .WithMany(mg => mg.MGs)
                    .HasForeignKey(m => m.MovieID);
            builder.Entity<MovieGenre>()
                    .HasOne(m => m.Genre)
                    .WithMany(mg => mg.MGs)
                    .HasForeignKey(m => m.GenreID);

            builder.Entity<MovieUser>().HasKey(mg => new { mg.MovieID, mg.UserID });
            builder.Entity<MovieUser>()
                    .HasOne(m => m.Movie)
                    .WithMany(f => f.Follows)
                    .HasForeignKey(m => m.MovieID);
            builder.Entity<MovieUser>()
                    .HasOne(u => u.User)
                    .WithMany(f => f.Follows)
                    .HasForeignKey(u => u.UserID);
        }
    }
}
