using System.Collections.Generic;

namespace TheatreWebApp.Services.Seats.Models
{
    public class StageServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SelectedSeatId { get; set; }

        public string SelectedSeats { get; set; }

        public int CurrentPage { get; set; } = 1;

        public List<SeatServiceModel> Seats { get; set; }

        public List<int> Rows { get; set; }

    }
}
