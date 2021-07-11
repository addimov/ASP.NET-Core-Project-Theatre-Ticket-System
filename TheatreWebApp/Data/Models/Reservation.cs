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

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int ShowId { get; set; }

        public Show Show { get; set; }

        public int SeatId { get; set; }

        public Seat Seat { get; set; }

        public decimal TotalPrice { get; init; }

        public int ReservationStatusId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }

    }

}
