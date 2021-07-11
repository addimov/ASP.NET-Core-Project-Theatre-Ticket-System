using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public string Name { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<Seat> Seats { get; set; }
         
    }
}
