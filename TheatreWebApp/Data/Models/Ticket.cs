using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Data.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.Reservations = new List<Reservation>();
        }

        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public User User { get; set; }

        public int? ShowId { get; set; }

        public Show Show { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public int ReservationStatusId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }

    }
}
