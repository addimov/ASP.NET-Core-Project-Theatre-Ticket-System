using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Seats
{
    public class SeatServiceModel
    {
        public int Id { get; init; }

        public int Number { get; set; }

        public int Row { get; set; }

        public decimal Price { get; set; }

        public bool IsSelected { get; set; } = false;

        public bool IsTaken { get; set; } = false;
    }
}
