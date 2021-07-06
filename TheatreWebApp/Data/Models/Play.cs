using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; init; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
