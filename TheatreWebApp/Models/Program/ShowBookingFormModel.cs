using System.Collections.Generic;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Models.Program
{
    public class ShowBookingFormModel
    {
        public int ShowId { get; set; }

        public string PlayName { get; set; }

        public string StageName { get; set; }

        public string Time { get; set; }

        public IEnumerable<Seat> TakenSeats { get; set; }

        public IEnumerable<Seat> AvailableSeats { get; set; }

        public IEnumerable<int> SeatSelection { get; set; }

        public IEnumerable<Reservation> BookedSeats { get; set; }

        public Ticket Ticket { get; set; }

        public string TicketId { get; set; }
    }
}

