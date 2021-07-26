using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Models.Stages;
using TheatreWebApp.Services.Seats;

namespace TheatreWebApp.Models.Tickets
{
    public class BookingFormModel
    {
        public int ShowId { get; set; }

        public string PlayName { get; set; }

        public string StageName { get; set; }

        public string Time { get; set; }

        public IEnumerable<SeatViewModel> Seats { get; set; }

        public int SelectedSeatId { get; set; }

        public string SelectedSeats { get; set; }

        public int CurrentPage { get; set; } = 1;

    }
}
