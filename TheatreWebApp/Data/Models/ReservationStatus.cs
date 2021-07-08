using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class ReservationStatus
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }
    }
}
