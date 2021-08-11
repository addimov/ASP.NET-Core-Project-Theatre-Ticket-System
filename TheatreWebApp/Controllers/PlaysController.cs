using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Models.Plays;
using TheatreWebApp.Services.Plays;
using TheatreWebApp.Infrastructure;

namespace TheatreWebApp.Controllers
{
    public class PlaysController : Controller
    {
        private readonly IPlayService plays;

        public PlaysController(IPlayService plays)
        {
            this.plays = plays;
        }

        public IActionResult All([FromQuery] PlayQueryModel query)
        {
            var showHidden = false;

            if (this.User.IsAdmin())
            {
                showHidden = true;
            }
            var playsResult = plays.All(query.SearchTerm, query.CurrentPage, showHidden, PlayQueryModel.PlaysPerPage);

            query.TotalPlays = playsResult.TotalPlays;
            query.SearchTerm = playsResult.SearchTerm;
            query.CurrentPage = playsResult.CurrentPage;
            query.Plays = playsResult.Plays;

            return View(query);
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

            this.plays.Add(play.Name, play.Description, play.ShortDescription, play.Credits, play.ImageUrl);
            
            return RedirectToAction("All");
        }

        public IActionResult Details(int playId)
        {
            var play = this.plays.Details(playId);

            return View(play);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Hide(int playId)
        {
            this.plays.ChangeVisibility(playId);

            return RedirectToAction("Details", new { playId = playId});
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int playId)
        {
            var play = this.plays.FormDetails(playId);

            return View(play);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(PlayFormModel play)
        {
            this.plays.Edit(play);
           
            return RedirectToAction("Details", new { playId = play.Id});
        }


    }
}
