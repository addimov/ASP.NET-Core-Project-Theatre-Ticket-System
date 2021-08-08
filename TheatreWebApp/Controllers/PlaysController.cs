using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult All([FromQuery] PlayQueryModel playsForm)
        {
            var playsQuery = data.Plays.OrderByDescending(p => p.Id).AsQueryable();

            if(playsForm.SearchTerm != null)
            {
                playsQuery = playsQuery.Where(p => p.Name.Contains(playsForm.SearchTerm) || p.Description.Contains(playsForm.SearchTerm) || p.ShortDescription.Contains(playsForm.SearchTerm));
            }

            playsForm.TotalPlays = playsQuery.Count();

            playsQuery = playsQuery
                .Skip((playsForm.CurrentPage - 1) * PlayQueryModel.PlaysPerPage)
                .Take(PlayQueryModel.PlaysPerPage);

            var plays = playsQuery
                .Select(p => new PlaysListViewModel 
                {
                    Name = p.Name,                 
                    ShortDescription = p.ShortDescription,
                    ImageUrl = p.ImageUrl,
                    Id = p.Id
                })
                .ToList();

            playsForm.Plays = plays;

            return View(playsForm);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Add(PlayFormModel play)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var playToAdd = new Play
            {
                Name = play.Name,
                Description = play.Description,
                ShortDescription = play.ShortDescription,
                Credits = play.Credits,
                ImageUrl = play.ImageUrl
            };

            if(playToAdd.ImageUrl == null)
            {
                playToAdd.ImageUrl = "https://inventionland.com/wp-content/uploads/2018/04/theater-stage.jpg";
            }


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
                    Credits = p.Credits,
                    ImageUrl = p.ImageUrl,
                    IsHidden = p.IsHidden,
                    Paragraphs = SplitIntoParagraphs(p.Description)
                })
                .FirstOrDefault();

            return View(play);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Hide(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => p)
                .FirstOrDefault();

            if(play.IsHidden == false)
            {
                play.IsHidden = true;
            }
            else
            {
                play.IsHidden = false;
            }

            data.Plays.Update(play);
            data.SaveChanges();

            return RedirectToAction("Details", new { playId = playId});
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int playId)
        {
            var play = data.Plays
                .Where(p => p.Id == playId)
                .Select(p => new PlayFormModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    ShortDescription = p.ShortDescription,
                    Credits = p.Credits,
                    ImageUrl = p.ImageUrl,
                    Id = playId
                })
                .FirstOrDefault();

            return View(play);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(PlayFormModel play)
        {
            var playUpdated = data.Plays
                .Where(p => p.Id == play.Id)
                .Select(p => p)
                .FirstOrDefault();

            playUpdated.Name = play.Name;
            playUpdated.ShortDescription = play.ShortDescription;
            playUpdated.Description = play.Description;
            playUpdated.Credits = play.Credits;
            playUpdated.ImageUrl = play.ImageUrl;

            data.Plays.Update(playUpdated);
            data.SaveChanges();
           
            return RedirectToAction("Details", new { playId = play.Id});
        }

        private static IEnumerable<string> SplitIntoParagraphs(string text)
        {
            var splittedText = text.Split('\n').ToList();

            return splittedText;
        }
    }
}
