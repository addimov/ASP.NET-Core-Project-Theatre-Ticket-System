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
                new Stage { Name = "Big stage" },
                new Stage { Name = "Small stage" },
            });

            data.SaveChanges();

        }
    }
}