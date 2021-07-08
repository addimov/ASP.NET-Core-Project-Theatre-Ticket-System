using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Stage
    {
        public Stage()
        {
            this.Seats = new HashSet<Seat>();
            this.Shows = new HashSet<Show>();
        }

        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Seat> Seats { get; set; }

        public IEnumerable<Show> Shows { get; set; }
    }
}
