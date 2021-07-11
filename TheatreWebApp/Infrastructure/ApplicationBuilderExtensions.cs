using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<TheatreDbContext>();

            data.Database.Migrate();

            SeedStages(data);

            SeedSeats(data);

            return app;
        }

        private static void SeedStages(TheatreDbContext data)
        {
            if (data.Stages.Any())
            {
                return;
            }

            data.Stages.AddRange(new[]
            {
                new Stage { Name = "Big stage", MaxSeats = 40 },
                new Stage { Name = "Small stage", MaxSeats = 20 },
            });

            data.SaveChanges();

        }

        private static void SeedSeats(TheatreDbContext data)
        {
            if (data.Seats.Any())
            {
                return;
            }

            int maxSeatsBig = data.Stages.Where(s => s.Name == "Big stage").Select(s => s.MaxSeats).FirstOrDefault();

            var category = new SeatCategory {Name = "Standard", Price = 20.00m};

            for (int i = 1; i <= maxSeatsBig; i++)
            {
                var seat = new Seat
                {
                    Number = i,
                    SeatCategory = category,
                    StageId = 1
                };

                if(i <= 10)
                {
                    seat.Row = 1;
                } 
                else if(i <= 20)
                {
                    seat.Row = 2;
                }
                else if(i <= 30)
                {
                    seat.Row = 3;
                } else
                {
                    seat.Row = 4;
                }

                data.Seats.Add(seat);

            }

            data.SaveChanges();

        }
    }
}