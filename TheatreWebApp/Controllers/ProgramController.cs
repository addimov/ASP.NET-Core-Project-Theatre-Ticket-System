using System;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public IActionResult All([FromQuery]ShowQueryModel showForm)
        {
            var showQuery = data.Shows.OrderByDescending(s => s.Time).AsQueryable();

            if(showForm.PlayId != 0)
            {
                showQuery = showQuery.Where(s => s.PlayId == showForm.PlayId);
            }
            if(showForm.SearchTerm != null)
            {
                showQuery = showQuery.Where(s => s.Play.Name.Contains(showForm.SearchTerm) || s.Play.ShortDescription.Contains(showForm.SearchTerm));
            }
            if(showForm.AfterDate != null)
            {
                var date = GetDateFromQuery(showForm.AfterDate);

                showQuery = showQuery.Where(s => s.Time > date);
            }
            if (showForm.BeforeDate != null)
            {
                var date = GetDateFromQuery(showForm.BeforeDate);

                showQuery = showQuery.Where(s => s.Time < date);
            }


            var shows = showQuery
                .Select(s => new ShowViewModel
                {
                    Id = s.Id,
                    PlayName = s.Play.Name,
                    PlayId = s.Play.Id,
                    StageName = s.Stage.Name,
                    ImageUrl = s.Play.ImageUrl,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time)
                })
                .ToList();

            var plays = data.Plays
                .Where(p => p.IsHidden == false)
                .Select(p => new ShowQueryPlayModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            showForm.Shows = shows;
            showForm.Plays = plays;

            return View(showForm);
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

        public IActionResult Edit(int showId)
        {
            var show = data.Shows
                .Where(s => s.Id == showId)
                .Select(s => new EditShowFormModel
                {
                    Id = showId
                })
                .FirstOrDefault();

            return View(show);
        }

        [HttpPost]
        public IActionResult Edit(EditShowFormModel showForm)
        {
            var show = data.Shows
                .Where(s => s.Id == showForm.Id)
                .Select(s => s)
                .FirstOrDefault();

            show.Time = GetShowTime(showForm.Date, showForm.Time);

            data.Shows.Update(show);
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

        private static DateTime GetDateFromQuery(string query)
        {
            var currentMonth = DateTime.UtcNow.Month.ToString("d2");
            var currentYear = DateTime.UtcNow.Year.ToString("d4");

            if(query.Length < 3)
            {
                var sb = new StringBuilder();
                sb.Append(query);
                sb.Append("/");
                sb.Append(currentMonth);
                sb.Append("/");
                sb.Append(currentYear);

                var date = sb.ToString();

                var isValid = DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var showTime);

                if (!isValid)
                {
                    return DateTime.UtcNow;
                }

                return showTime;
            } 
            else if(query.Length < 6)
            {
                var sb = new StringBuilder();
                sb.Append(query);
                sb.Append("/");
                sb.Append(currentYear);

                var date = sb.ToString();

                var isValid = DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var showTime);

                if (!isValid)
                {
                    return DateTime.UtcNow;
                }

                return showTime;
            } 
            else
            {

                var isValid = DateTime.TryParseExact(query, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var showTime);

                if (!isValid)
                {
                    return DateTime.UtcNow;
                }

                return showTime;
            }
        }

    }
}
