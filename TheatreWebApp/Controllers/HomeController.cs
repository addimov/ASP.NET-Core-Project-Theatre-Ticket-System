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

        public IActionResult Error()
        {
            return View();
        }
    }
}
