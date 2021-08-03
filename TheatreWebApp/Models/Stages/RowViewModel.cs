using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Stages
{
    public class RowViewModel
    {
        public int RowId { get; set; }

        public IEnumerable<SeatViewModel> Seats { get; set; }

    }
}
