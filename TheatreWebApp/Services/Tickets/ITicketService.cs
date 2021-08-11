﻿using System.Collections.Generic;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Services.Tickets.Model;

namespace TheatreWebApp.Services.Tickets
{
    public interface ITicketService
    {
        IEnumerable<TicketServiceModel> AllByUser(string userId);

        IEnumerable<Reservation> ReserveSeats(int showId, string selectedSeats);

        string Create(int showId, IEnumerable<Reservation> reservations, string userId);

        TicketServiceModel Review(string ticketId);

        void Confirm(string ticketId, int action);
    }
}