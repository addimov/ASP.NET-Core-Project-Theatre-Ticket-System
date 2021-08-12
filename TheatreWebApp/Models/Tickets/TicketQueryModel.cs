using System.Collections.Generic;
using TheatreWebApp.Services.Tickets.Model;

namespace TheatreWebApp.Models.Tickets
{
    public class TicketQueryModel
    {
        public const int TicketsPerPage = 6;

        public int CurrentPage { get; set; } = 1;

        public int TotalTickets { get; set; }

        public bool ShowAll { get; set; } = false;

        public IEnumerable<TicketServiceModel> Tickets { get; set; }
    }
}
