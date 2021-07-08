using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
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
            var playsQuery = data.Plays.AsQueryable();

            var plays = playsQuery.Select(p => p).ToList();

            return View(plays);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddPlayFormModel play)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var playToAdd = new Play
            {
                Name = play.Name,
                Description = play.Description,
                ShortDescription = play.ShortDescription
            };


            data.Plays.Add(playToAdd);
            data.SaveChanges();

            return RedirectToAction("All");
        }

        public IActionResult Details(int id)
        {
            var play = data.Plays
                .Where(p => p.Id == id)
                .Select(p => new PlayDetailsViewModel
                {
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    Paragraphs = SplitIntoParagraphs(p.Description)
                })
                .FirstOrDefault();

            return View(play);
        }

        private static IEnumerable<string> SplitIntoParagraphs(string text)
        {
            var splittedText = text.Split('\n').ToList();

            return splittedText;
        }
    }
}
