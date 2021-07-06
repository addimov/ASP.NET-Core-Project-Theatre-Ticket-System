using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Stage> Stages { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Play> Plays { get; set; }

        public DbSet<Show> Shows { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
