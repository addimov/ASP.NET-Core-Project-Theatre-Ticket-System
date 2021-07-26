namespace TheatreWebApp.Models.Stages
{
    public class SeatViewModel
    {
        public int Id { get; init; }

        public int Number { get; set; }

        public int Row { get; set; }

        public bool IsSelected { get; set; } = false;

        public bool IsTaken { get; set; } = false;
    }
}
