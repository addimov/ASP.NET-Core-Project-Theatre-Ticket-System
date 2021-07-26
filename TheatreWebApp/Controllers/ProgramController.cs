using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
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
            var shows = data.Shows
                .OrderByDescending(s => s.Time)
                .Select(s => new ShowViewModel
                {
                    Id = s.Id,
                    PlayName = s.Play.Name,
                    StageName = s.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time)
                })
                .ToList();

            return View(shows);
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
            if(!data.Plays.Any(p => p.Id == show.PlayId))
            {
                this.ModelState.AddModelError(nameof(show.PlayId), "Play does not exist.");
            }

            if (!data.Stages.Any(s => s.Id == show.StageId))
            {
                this.ModelState.AddModelError(nameof(show.StageId), "Stage does not exist.");
            }


            if (!ModelState.IsValid)
            {
                show.Plays = data.Plays.Select(p => p).ToList();
                show.Stages = data.Stages.Select(st => st).ToList();

                return View(show);
            }

            var showToAdd = new Show
            {
                Play = data.Plays.Find(show.PlayId),
                Stage = data.Stages.Find(show.StageId),
                Time = GetShowTime(show.Date, show.Time)
            };

            data.Shows.Add(showToAdd);
            data.SaveChanges();

            return RedirectToAction("All");
        }
        private static DateTime GetShowTime(string date, string hour)
        {
            var timeString = date + " " + hour;

            var isValid = DateTime.TryParseExact(timeString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var showTime);

            if (!isValid)
            {
                return DateTime.Parse("12/07/2021 19:00");
            }

            return showTime;
        }

    }
}
