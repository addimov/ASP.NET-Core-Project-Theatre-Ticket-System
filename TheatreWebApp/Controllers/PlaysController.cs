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

            var plays = playsQuery
                .Select(p => new AllPlaysViewModel 
                {
                    Name = p.Name,                 
                    ShortDescription = p.ShortDescription,
                    Id = p.Id
                })
                .ToList();


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

        public IActionResult Details(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => new PlayDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    Paragraphs = SplitIntoParagraphs(p.Description)
                })
                .FirstOrDefault();

            return View(play);
        }


        public IActionResult Edit(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => new AddPlayFormModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    ShortDescription = p.ShortDescription,
                    Id = playId
                })
                .FirstOrDefault();

            return View(play);
        }

        [HttpPost]
        public IActionResult Edit(AddPlayFormModel play)
        {
            var playUpdated = data.Plays
                .Where(p => p.Id == play.Id)
                .Select(p => p)
                .FirstOrDefault();

            playUpdated.Name = play.Name;
            playUpdated.ShortDescription = play.ShortDescription;
            playUpdated.Description = playUpdated.Description;

            data.Plays.Update(playUpdated);
            data.SaveChanges();
           
            return RedirectToAction("Details", new { play.Id});
        }

        private static IEnumerable<string> SplitIntoParagraphs(string text)
        {
            var splittedText = text.Split('\n').ToList();

            return splittedText;
        }
    }
}
