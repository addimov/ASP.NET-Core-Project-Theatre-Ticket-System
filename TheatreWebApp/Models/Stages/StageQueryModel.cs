using System.Collections.Generic;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Models.Stages
{
    public class StageQueryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SeatViewModel> Seats { get; set; }

        public int SelectedSeatId { get; set; }

        public string SelectedSeats { get; set; }

        public int CurrentPage { get; set; } = 1;

    }
}
