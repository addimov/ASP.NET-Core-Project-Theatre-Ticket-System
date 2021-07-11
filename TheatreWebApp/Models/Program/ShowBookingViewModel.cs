using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Program
{
    public class ShowBookingViewModel
    {
        public int Id { get; set; }

        public string PlayName { get; set; }

        public string StageName { get; set; }

        public string Time { get; set; }

        public int AvalaibleSeats { get; set; }

    }
}

