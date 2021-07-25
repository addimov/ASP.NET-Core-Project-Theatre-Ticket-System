using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Models.Stages;

namespace TheatreWebApp.Services.Seats
{
    public class SelectionServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SeatViewModel> Seats { get; set; }

        public int SelectedSeatId { get; set; }

        public string SelectedSeats { get; set; }

        public int CurrentPage { get; set; } = 1;

    }
}
