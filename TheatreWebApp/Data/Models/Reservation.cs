using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Reservation
    {
     
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public int? SeatId { get; set; }

        public Seat Seat { get; set; }

        public decimal? Price { get; init; }

        public string TicketId { get; set; }

        public Ticket Ticket { get; set; }

    }

}
