using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Data.Models
{
    public class OnlineTicket
    {
        public OnlineTicket()
        {
            this.Reservations = new List<Reservation>();
        }

        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }

        public int ReservationStatusId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }

    }
}
