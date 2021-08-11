using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Seats.Models
{
    public class StageListServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SeatCount { get; set; }
    }
}
