using System;
using System.Collections.Generic;
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
            var showsQuery = data.Shows.AsQueryable();

            var shows = showsQuery
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

        
        public IActionResult BookSeats(int id)
        {
            var show = PrepareShowBookingForm(data, id);


            return View(show);
        }

        [HttpPost]
        public IActionResult BookSeats(ShowBookingFormModel show)
        {
            var ticket = new OnlineTicket { ReservationStatusId = 4 };

            var reservations = new List<Reservation>();

            foreach (var seat in show.SeatSelection)
            {
                reservations.Add(new Reservation { SeatId = seat, ShowId = show.Id, });
            }

            ticket.Reservations = reservations;

            data.OnlineTickets.Add(ticket);
            data.SaveChanges();

            var showQuery = PrepareShowBookingForm(data, show.Id);

            show.PlayName = showQuery.PlayName;
            show.StageName = showQuery.StageName;
            show.Time = showQuery.Time;
            show.TakenSeats = showQuery.TakenSeats;
            show.AvailableSeats = showQuery.AvailableSeats;
            show.TicketId = ticket.Id;

            return View(show);
        }

        public IActionResult Confirm(string id)
        {
            var ticket = data.Reservations
                .Where(r => r.TicketId == id)
                .Select(t => new TicketViewModel
                {
                    PlayName = t.Show.Play.Name.ToString(),
                    StageName = t.Show.Stage.Name.ToString(),
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.Show.Time)
                })
                .FirstOrDefault();

            var reservations = data.Reservations
                .Where(r => r.TicketId == id)
                .Select(r => r)
                .ToList();

            var prices = data.Reservations
                .Where(r => r.TicketId == id)
                .Select(r => r.Seat.SeatCategory.Price)
                .ToList();

            var seatNumbers = data.Reservations
                .Where(r => r.TicketId == id)
                .Select(r => r.Seat.Number)
                .ToList();

            ticket.Reservations = reservations;
            ticket.SeatNumbers = string.Join(", ", seatNumbers);

            var total = 0.0m;

            foreach (var price in prices)
            {
                total += price;
            }

            ticket.TotalPrice = total;

            return View(ticket);
        }

        [HttpPost]
        public IActionResult Confirm()
        {
            
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

        private static IEnumerable<Seat> GetAvailableSeats(TheatreDbContext data, int ShowId, IEnumerable<int> takenSeats)
        {
            var allSeatsQuery = data.Shows.AsQueryable();

            var allSeats = allSeatsQuery
                .Where(sh => sh.Id == ShowId)
                .Select(sh => sh.Stage.Seats.ToList())
                .FirstOrDefault();

            var availableSeats = new List<Seat>();

            if (takenSeats == null)
            {
                foreach (var seat in allSeats)
                {
                    availableSeats.Add(seat);
                }

                return availableSeats;
            }


            foreach (var seat in allSeats)
            {
                if (!takenSeats.Contains(seat.Number))
                {
                    availableSeats.Add(seat);
                }
            }

         
            return availableSeats;
        }

        private static IEnumerable<int> GetTakenSeats(TheatreDbContext data, int ShowId)
        {
            if (data.Reservations.Any())
            {
                var takenSeats = data.Reservations.Where(r => r.ShowId == ShowId).Select(r => r.Seat.Number).ToList();

                return takenSeats;
            }

            

            return null;
        }

        private static ShowBookingFormModel PrepareShowBookingForm(TheatreDbContext data, int showId)
        {
            var takenSeats = GetTakenSeats(data, showId);
            var availableSeats = GetAvailableSeats(data, showId, takenSeats).OrderBy(x => x.Number).ToList();

            var takenSeatsString = "-";

            if (takenSeats != null)
            {
                takenSeatsString = string.Join(", ", takenSeats);
            }

            var show = data.Shows
                .Where(s => s.Id == showId)
                .Select(s => new ShowBookingFormModel
                {
                    PlayName = s.Play.Name,
                    StageName = s.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time),
                    TakenSeats = takenSeatsString,
                    AvailableSeats = availableSeats,
                })
                .FirstOrDefault();

            return show;
        }
    }
}
