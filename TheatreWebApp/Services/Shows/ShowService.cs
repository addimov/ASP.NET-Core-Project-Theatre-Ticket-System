using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Shows;
using TheatreWebApp.Services.Shows.Models;

namespace TheatreWebApp.Services.Shows
{
    public class ShowService : IShowService
    {
        private readonly TheatreDbContext data;

        public ShowService(TheatreDbContext data)
        {
            this.data = data;
        }

        public void Add(
            int playId, 
            int stageId, 
            string date, 
            string time)
        {
            var show = new Show
            {
                Play = data.Plays.Find(playId),
                Stage = data.Stages.Find(stageId),
                Time = GetShowTime(date, time)
            };

            data.Shows.Add(show);
            data.SaveChanges();
        }

        public ShowQueryModel All( 
            int playId = 0, 
            string searchTerm = null, 
            string afterDate = null, 
            string beforeDate = null, 
            int currentPage = 1,
            int showsPerPage = int.MaxValue,
            bool showOlder = false)
        {
            

            var showQuery = data.Shows
                .OrderByDescending(s => s.Time)
                .AsQueryable();
            
            if(showOlder == false)
            {
                showQuery = showQuery.Where(s => s.Time > DateTime.Today.AddDays(-1)).AsQueryable();
            }


            if (playId != 0)
            {
                showQuery = showQuery.Where(s => s.PlayId == playId);
            }
            if (searchTerm != null)
            {
                showQuery = showQuery.Where(s => s.Play.Name.Contains(searchTerm) || s.Play.ShortDescription.Contains(searchTerm));
            }
            if (afterDate != null)
            {
                var date = GetDateFromQuery(afterDate);

                showQuery = showQuery.Where(s => s.Time > date);
            }
            if (beforeDate != null)
            {
                var date = GetDateFromQuery(beforeDate);

                showQuery = showQuery.Where(s => s.Time < date);
            }

            var totalShows = showQuery.Count();

            showQuery = showQuery
                .Skip((currentPage - 1) * showsPerPage)
                .Take(showsPerPage);

            var showsList = showQuery
                .Select(s => new ShowServiceModel
                {
                    Id = s.Id,
                    PlayName = s.Play.Name,
                    PlayId = s.Play.Id,
                    StageName = s.Stage.Name,
                    ImageUrl = s.Play.ImageUrl,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time),
                    IsAvailable = s.Time > DateTime.UtcNow.AddHours(3)
                })
                .ToList();

            var activePlays = data.Shows.Select(s => s.PlayId).Distinct().ToList();

            var playsList = data.Plays
                .Where(p => activePlays.Contains(p.Id))
                .Select(p => new ShowServicePlayModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            var showsModel = new ShowQueryModel
            {
                PlayId = playId,
                CurrentPage = currentPage,
                SearchTerm = searchTerm,
                AfterDate = afterDate,
                BeforeDate = beforeDate,
                TotalShows = totalShows,
                Shows = showsList,
                Plays = playsList
            };

            return showsModel;
        }

        public bool IsDateValid(string date)
        {
            var isValid = DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var showDate);

            if (!isValid)
            {
                return false;
            }

            if(showDate < DateTime.Today)
            {
                return false;
            }

            return isValid;
        }

        public AddShowFormModel PrepareForm()
        {
            var show = new AddShowFormModel
            {
                Plays = data.Plays.Select(p => p).ToList(),
                Stages = data.Stages.Select(st => st).ToList()
            };

            return show;
        }

        public EditShowFormModel PrepareEditForm(int showId)
        {
            var show = data.Shows
                .Where(s => s.Id == showId)
                .Select(s => new EditShowFormModel
                {
                    Id = showId
                })
                .FirstOrDefault();

            return show;
        }

        public void Edit(EditShowFormModel showForm)
        {
            var show = data.Shows
                .Where(s => s.Id == showForm.Id)
                .Select(s => s)
                .FirstOrDefault();

            show.Time = GetShowTime(showForm.Date, showForm.Time);

            data.Shows.Update(show);
            data.SaveChanges();
        }

        public bool PlayExists(int playId)
        {
            return data.Plays.Any(p => p.Id == playId);
        }

        public bool StageExists(int stageId)
        {
            return data.Stages.Any(p => p.Id == stageId);
        }

        private static DateTime GetDateFromQuery(string query)
        {
            var currentMonth = DateTime.UtcNow.Month.ToString("d2");
            var currentYear = DateTime.UtcNow.Year.ToString("d4");

            var sb = new StringBuilder();

            if (query.Length < 3)
            {             
                sb.Append(query);
                sb.Append("/");
                sb.Append(currentMonth);
                sb.Append("/");
                sb.Append(currentYear);;
            }
            else if (query.Length < 6)
            {
                sb.Append(query);
                sb.Append("/");
                sb.Append(currentYear);
            }

            var date = sb.ToString();

            var isValid = DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var showTime);

            if (!isValid)
            {
                return DateTime.UtcNow;
            }

            return showTime;
        }


        public bool IsShowAvailable(int showId)
        {
            var showTime = data.Shows
                .Where(s => s.Id == showId)
                .Select(s => s.Time)
                .FirstOrDefault();

            if(showTime > DateTime.Today.AddDays(-1))
            {
                return true;
            }

            return false;
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
