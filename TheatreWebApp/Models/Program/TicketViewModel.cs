using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Models.Program
{
    public class TicketViewModel
    {

        public string PlayName { get; set; }

        public string StageName { get; set; }

        public string Time { get; set; }

        public string SeatNumbers { get; set; }

        public decimal TotalPrice { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
