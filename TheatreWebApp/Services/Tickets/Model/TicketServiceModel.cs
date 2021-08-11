using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Tickets.Model
{
    public class TicketServiceModel
    {
        public string TicketId { get; set; }

        public int ShowId { get; set; }

        public string PlayName { get; set; }

        public string StageName { get; set; }

        public string Time { get; set; }

        public IEnumerable<int> SeatNumbers { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public string CreatedOn { get; set; }
    }
}
