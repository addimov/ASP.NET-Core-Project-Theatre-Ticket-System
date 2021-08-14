using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Infrastructure;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Shows;
using TheatreWebApp.Services.Tickets;

using static TheatreWebApp.WebConstants;

namespace TheatreWebApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ISelectionService selection;
        private readonly ITicketService tickets;
        private readonly IShowService shows;

        public TicketsController(ISelectionService selection, ITicketService tickets, IShowService shows)
        {
            this.selection = selection;
            this.tickets = tickets;
            this.shows = shows;
        }

        [Authorize]
        public IActionResult All([FromQuery]TicketQueryModel query)
        {
            var tickets = this.tickets
                .AllByUser(this.User.Id(), query.CurrentPage, TicketQueryModel.TicketsPerPage, query.ShowAll)
                .ToList();

            query.Tickets = tickets;
            query.TotalTickets = tickets.Count();

            return View(query);
        }

        [Authorize]
        public IActionResult SelectSeats(int showId)
        {
            if (!this.shows.IsShowAvailable(showId))
            {
                return BadRequest();
            }

            return View(selection.GetSeatingChart(showId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult SelectSeats(BookingFormModel bookingChart)
        {
            if (selection.IsSeatTaken(bookingChart.SelectedSeatId, bookingChart.ShowId))
            {
                this.TempData[GlobalMessageKey] = "Selected seat has already been booked. Please choose another.";

                return RedirectToAction("SelectSeats", new { showId = bookingChart.ShowId });
            }

            return View(selection.GetSeatingChart(bookingChart));
        }

        [Authorize]
        public IActionResult Review(int showId, string selectedSeats)
        {
            var reservations = this.tickets.ReserveSeats(showId, selectedSeats);
            
            if(reservations == null)
            {
                this.TempData[GlobalMessageKey] = "Selected seat has already been booked. Please choose another.";

                return RedirectToAction("SelectSeats", new { showId = showId });
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

        [Authorize]
        public IActionResult Print(string ticketId)
        {
            var userId = this.User.Id();

            var isUserAuthorized = this.tickets.Authorize(userId, ticketId);

            if (!isUserAuthorized)
            {
                return Unauthorized();
            }

            var order = this.tickets.ToPrint(ticketId);

            return View(order);
        }

    }
}
