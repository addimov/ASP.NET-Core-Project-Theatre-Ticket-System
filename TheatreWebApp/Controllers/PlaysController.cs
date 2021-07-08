using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Models.Plays;

namespace TheatreWebApp.Controllers
{
    public class PlaysController : Controller
    {
        private readonly TheatreDbContext data;

        public PlaysController(TheatreDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddPlayFormModel play)
        {
            return View();
        }
    }
}
