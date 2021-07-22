using System.Collections.Generic;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Models.Stages
{
    public class StageConfigModel
    {
        public int Id { get; set; }

        public IEnumerable<Stage> Stages { get; set; }

        public IEnumerable<SeatViewModel> Seats { get; set; }

        public int SelectedSeatId { get; set; }

        public string SelectedSeats { get; set; }

        public int CurrentPage { get; init; } = 1;

    }
}
