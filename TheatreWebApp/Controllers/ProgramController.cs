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

        public IActionResult BookSeats(int showId)
        {
            var show = PrepareShowBookingForm(data, showId);

            return View(show);
        }

        [HttpPost]
        public IActionResult BookSeats(ShowBookingFormModel show)
        {
          
            var ticket = new Ticket
            {
                ShowId = show.ShowId,
                ReservationStatus = data.ReservationStatuses.Find(4),
            };

            var reservations = new List<Reservation>();

            foreach (var seatId in show.SeatSelection)
            {
                reservations.Add(new Reservation
                {
                    SeatId = seatId,
                    Price = data.Seats.Select(s => s.SeatCategory).Select(c => c.Price).FirstOrDefault()
                });
            }

            ticket.Reservations = reservations;

            data.Tickets.Add(ticket);
            data.SaveChanges();

            show.Ticket = ticket;

            var showQuery = PrepareShowBookingForm(data, show.ShowId);

            show.PlayName = showQuery.PlayName;
            show.StageName = showQuery.StageName;
            show.Time = showQuery.Time;
            show.TakenSeats = showQuery.TakenSeats;
            show.AvailableSeats = showQuery.AvailableSeats;
            show.Ticket = ticket;
            show.TicketId = ticket.Id;

            return View(show);
        }

        public IActionResult Review(string ticketId)
        {
            var ticket = data.Tickets
                .Where(t => t.Id == ticketId)
                .Select(t => new TicketViewModel
                {
                    ShowId = t.Show.Id,
                    TicketId = t.Id,
                    PlayName = t.Show.Play.Name,
                    StageName = t.Show.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.Show.Time),
                    SeatNumbers = t.Reservations.Select(r => r.Seat).Select(s => s.Number).ToList(),
                    TotalPrice = t.Reservations.Select(r => r.Price).Sum().GetValueOrDefault()
                })
                .FirstOrDefault();

            return View(ticket);
        }


        public IActionResult Confirm(string ticketId)
        {
            var ticket = data.Tickets.Find(ticketId);

            ticket.ReservationStatusId = 2;

            data.Tickets.Update(ticket);
            data.SaveChanges();

            return RedirectToAction("All");
        }

        public IActionResult Discard(string ticketId)
        {
            var showId = data.Tickets
                .Where(t => t.Id == ticketId)
                .Select(s => s.ShowId)
                .FirstOrDefault();

            var ticket = data.Tickets.Find(ticketId);
            var reservations = data.Reservations.Where(r => r.TicketId == ticketId).Select(r => r).ToList();

            data.Reservations.RemoveRange(reservations);
            data.Tickets.Remove(ticket);
            data.SaveChanges();

            return RedirectToAction("BookSeats", new { showId });
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

        

        private static IEnumerable<Seat> GetTakenSeats(TheatreDbContext data, int showId)
        {
            var showTakenSeats = data.Reservations
                .Where(r => r.Ticket.ShowId == showId)
                .Select(r => r.Seat)
                .ToList();

            if (showTakenSeats != null)
            {
                return showTakenSeats;
            }
            
            return null;
        }

        private static IEnumerable<Seat> GetAvailableSeats(TheatreDbContext data, int showId, IEnumerable<Seat> takenSeats)
        {           
            var allSeats = data.Shows
                .Where(sh => sh.Id == showId)
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
                if (!takenSeats.Contains(seat))
                {
                    availableSeats.Add(seat);
                }
            }

            return availableSeats;
        }

        private static ShowBookingFormModel PrepareShowBookingForm(TheatreDbContext data, int showId)
        {
            var takenSeats = GetTakenSeats(data, showId).ToList();
            var availableSeats = GetAvailableSeats(data, showId, takenSeats).OrderBy(x => x.Number).ToList();

            var show = data.Shows
                .Where(s => s.Id == showId)
                .Select(s => new ShowBookingFormModel
                {
                    ShowId = showId,
                    PlayName = s.Play.Name,
                    StageName = s.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time),
                    TakenSeats = takenSeats,
                    AvailableSeats = availableSeats,
                })
                .FirstOrDefault();

            return show;
        }
    }
}
