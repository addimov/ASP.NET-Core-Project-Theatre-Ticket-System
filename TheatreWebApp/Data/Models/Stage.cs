using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Stage
    {
        public Stage()
        {
            this.Seats = new HashSet<Seat>();
            this.Plays = new HashSet<Play>();
        }

        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Seat> Seats { get; set; }

        public IEnumerable<Play> Plays { get; set; }
    }
}
