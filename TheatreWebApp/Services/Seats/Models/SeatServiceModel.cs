namespace TheatreWebApp.Services.Seats.Models
{
    public class SeatServiceModel
    {
        public int Id { get; init; }

        public int Number { get; set; }

        public int Row { get; set; }

        public decimal Price { get; set; }

        public bool IsSelected { get; set; } = false;

        public bool IsTaken { get; set; } = false;
    }
}
