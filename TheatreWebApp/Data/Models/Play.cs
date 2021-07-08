using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
