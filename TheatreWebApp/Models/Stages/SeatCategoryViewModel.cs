using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Stages
{
    public class SeatCategoryViewModel
    {
        public string StageName { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string Seats { get; set; }

        public int SeatsCount { get; set; }

        public decimal Price { get; set; }
    }
}
