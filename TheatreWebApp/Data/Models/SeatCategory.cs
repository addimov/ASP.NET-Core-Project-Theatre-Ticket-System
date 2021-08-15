using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreWebApp.Data.Models
{
    public class SeatCategory
    {
        public SeatCategory()
        {
            this.Seats = new HashSet<Seat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public IEnumerable<Seat> Seats { get; set; }
         
    }
}
