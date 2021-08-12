using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Models.Tickets
{
    public class TicketViewModel
    {
        public string TicketId { get; set; }

        public string PlayName { get; set; }

        public string StageName { get; set; }

        public string Time { get; set; }

        public int SeatNumber { get; set; }

        public int Row { get; set; }

        public decimal Price { get; set; }

    }
}
