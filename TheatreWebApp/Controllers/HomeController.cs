using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Models;
using TheatreWebApp.Models.Home;

namespace TheatreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TheatreDbContext data;

        public HomeController(TheatreDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var plays = data.Plays
                .Select(p => new IndexPlayViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    ImageUrl = p.ImageUrl
                })
                .OrderByDescending(p => p.Id)
                .Take(3)
                .ToList();

            return View(plays);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
