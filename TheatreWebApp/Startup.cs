using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Infrastructure;
using TheatreWebApp.Services.Categories;
using TheatreWebApp.Services.Plays;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Shows;
using TheatreWebApp.Services.Tickets;

namespace TheatreWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddDbContext<TheatreDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("Connection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TheatreDbContext>();

            services.AddControllersWithViews(options => 
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>());

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddTransient<ISelectionService, SelectionService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPlayService, PlayService>();
            services.AddTransient<IShowService, ShowService>();
            services.AddTransient<ITicketService, TicketService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();
            });

        }
    }
}
