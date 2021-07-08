using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Models.Program;

namespace TheatreWebApp.Controllers
{
    public class ProgramController : Controller
    {
        private readonly TheatreDbContext data;

        public ProgramController(TheatreDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            return View();
        }

        public IActionResult Add()
        {
            var show = new AddShowFormModel
            {
                Plays = data.Plays.Select(p => p).ToList(),
                Stages = data.Stages.Select(st => st).ToList()
            }; 
            
            return View(show);
        }

        [HttpPost]
        public IActionResult Add(AddShowFormModel show)
        {
            if (!ModelState.IsValid)
            {
                show.Plays = data.Plays.Select(p => p).ToList();
                show.Stages = data.Stages.Select(st => st).ToList();

                return View(show);
            }



            return View();
        }
    }
}
