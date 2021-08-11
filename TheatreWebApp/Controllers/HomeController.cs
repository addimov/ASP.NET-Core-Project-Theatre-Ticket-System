using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Models;
using TheatreWebApp.Services.Plays;

namespace TheatreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayService plays;

        public HomeController(TheatreDbContext data, IPlayService plays)
        {
            this.plays = plays;
        }

        public IActionResult Index()
        {
            var plays = this.plays.Latest();

            return View(plays);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
