using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Seat
    {
        [Key]
        [Required]
        public int Id { get; init; }

        public int Number { get; set; }

        public int Row { get; set; }

        public int StageId { get; set; }

        public Stage Stage { get; set; }

        public int SeatCategoryId { get; set; }

        public SeatCategory SeatCategory { get; set; }

        public decimal Price { get; set; }
    }
}
