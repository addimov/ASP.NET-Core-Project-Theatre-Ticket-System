using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Data
{
    public class TheatreDbContext : IdentityDbContext<ApplicationUser>
    {
        public TheatreDbContext(DbContextOptions<TheatreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Stage> Stages { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Play> Plays { get; set; }

        public DbSet<Show> Shows { get; set; }

        public DbSet<ReservationStatus> ReservationStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
