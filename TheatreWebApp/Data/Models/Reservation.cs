using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreWebApp.Data.Models
{
    public class Reservation
    {
     
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public int? SeatId { get; set; }

        public Seat Seat { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; init; }

        public string TicketId { get; set; }

        public Ticket Ticket { get; set; }

        public int? ShowId { get; set; }

        public Show Show { get; set; }

    }

}
