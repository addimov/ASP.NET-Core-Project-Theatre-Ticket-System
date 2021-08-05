using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Stages;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats;

namespace TheatreWebApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TheatreDbContext data;
        private readonly ISelectionService selection;

        public TicketsController(TheatreDbContext data, ISelectionService selection)
        {
            this.data = data;
            this.selection = selection;
        }

        [Authorize]
        public IActionResult All()
        {
            var tickets = data.Tickets
                .Select(t => new TicketViewModel
                {
                    ShowId = t.Show.Id,
                    TicketId = t.Id,
                    PlayName = t.Show.Play.Name,
                    StageName = t.Show.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.Show.Time),
                    Status = t.ReservationStatus.Name,
                    TotalPrice = t.Reservations.Select(r => r.Price).Sum().GetValueOrDefault(),
                    SeatNumbers = t.Reservations.Select(r => r.Seat).Select(s => s.Number).ToList(),
                    CreatedOn = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.CreatedOn)
                })
                .ToList();

            return View(tickets);
        }

        [Authorize]
        public IActionResult SelectSeats(int showId)
        {

            return View(selection.GetSeatingChart(showId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult SelectSeats(BookingFormModel bookingChart)
        {

            return View(selection.GetSeatingChart(bookingChart));
        }

        [Authorize]
        public IActionResult Review(int showId, string selectedSeats)
        {
            var ticket = new Ticket
            {
                ShowId = showId,
                ReservationStatusId = data.ReservationStatuses.Where(s => s.Name == "Unconfirmed").Select(s => s.Id).FirstOrDefault()
            };


            var reservations = new List<Reservation>();

            var selectedSeatsList = selectedSeats.Split().Select(int.Parse).ToList();

            foreach (var seatId in selectedSeatsList)
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

            var ticketView = data.Tickets
                .Where(t => t.Id == ticket.Id)
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

            return View(ticketView);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Review(TicketFormModel ticketForm)
        {
            var ticket = data.Tickets
                .Where(t => t.Id == ticketForm.TicketId)
                .Select(t => t)
                .FirstOrDefault();

            if(ticketForm.Action == 1)
            {
                var reservations = data.Reservations.Where(r => r.TicketId == ticketForm.TicketId).Select(r => r).ToList();

                data.Reservations.RemoveRange(reservations);

                data.Tickets.Remove(ticket);
            }
            if(ticketForm.Action == 2)
            {
                ticket.ReservationStatusId = data.ReservationStatuses.Where(r => r.Name == "Paid").Select(r => r.Id).FirstOrDefault();
                data.Tickets.Update(ticket);
            }

            data.SaveChanges();

            return RedirectToAction("All");
        }

    }
}
