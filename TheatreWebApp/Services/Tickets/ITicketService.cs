using System.Collections.Generic;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Tickets.Model;

namespace TheatreWebApp.Services.Tickets
{
    public interface ITicketService
    {
        TicketQueryModel AllByUser(
            string userId, 
            int currentPage = 1, 
            int ticketsPerPage = int.MaxValue,
            bool showAll = false
            );

        IEnumerable<Reservation> ReserveSeats(int showId, string selectedSeats);

        string Create(int showId, IEnumerable<Reservation> reservations, string userId);

        TicketServiceModel Review(string ticketId);

        bool Confirm(string ticketId, int action);

        bool Authorize(string userId, string ticketId);

        IEnumerable<TicketViewModel> ToPrint(string ticketId);
    }
}
