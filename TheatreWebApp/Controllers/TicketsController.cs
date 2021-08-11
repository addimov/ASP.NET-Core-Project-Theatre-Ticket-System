﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Infrastructure;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Tickets;

namespace TheatreWebApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ISelectionService selection;
        private readonly ITicketService tickets;

        public TicketsController(ISelectionService selection, ITicketService tickets)
        {
            this.selection = selection;
            this.tickets = tickets;
        }

        [Authorize]
        public IActionResult All()
        {
            var tickets = this.tickets.AllByUser(this.User.Id());

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
            var reservations = this.tickets.ReserveSeats(showId, selectedSeats);
            
            if(reservations == null)
            {
                //add tempdata error message
                return View("SelectSeats", new { showId = showId });
            }

            var tickedId = this.tickets.Create(showId, reservations, this.User.Id());
           
            return View(this.tickets.Review(tickedId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Review(TicketFormModel ticketForm)
        {
            this.tickets.Confirm(ticketForm.TicketId, ticketForm.Action);

            return RedirectToAction("All");
        }

    }
}
