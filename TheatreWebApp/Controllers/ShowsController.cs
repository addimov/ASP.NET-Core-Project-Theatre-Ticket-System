using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Models.Shows;
using TheatreWebApp.Services.Shows;

namespace TheatreWebApp.Controllers
{
    public class ShowsController : Controller
    {
        private readonly IShowService shows;

        public ShowsController(IShowService shows)
        {
            this.shows = shows;
        }

        public IActionResult All([FromQuery]ShowQueryModel showQuery)
        {
            var shows = this.shows.All(
                showQuery.PlayId, 
                showQuery.SearchTerm, 
                showQuery.AfterDate, 
                showQuery.BeforeDate, 
                showQuery.CurrentPage, 
                ShowQueryModel.ShowsPerPage);

            return View(shows);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            var show = this.shows.PrepareForm();
            
            return View(show);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Add(AddShowFormModel show)
        {
            if(!this.shows.PlayExists(show.PlayId))
            {
                this.ModelState.AddModelError(nameof(show.PlayId), "Play does not exist.");
            }

            if (!this.shows.StageExists(show.StageId))
            {
                this.ModelState.AddModelError(nameof(show.StageId), "Stage does not exist.");
            }


            if (!ModelState.IsValid)
            {
                var showNew = this.shows.PrepareForm();

                show.Plays = showNew.Plays;
                show.Stages = showNew.Stages;

                return View(show);
            }

            this.shows.Add(show.PlayId, show.StageId, show.Date, show.Time);

            return RedirectToAction("All");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int showId)
        {
            var show = this.shows.PrepareEditForm(showId);

            return View(show);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(EditShowFormModel showForm)
        {
            this.shows.Edit(showForm);

            return RedirectToAction("All");
        }

    }
}
