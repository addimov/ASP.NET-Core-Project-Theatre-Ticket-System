using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Categories
{
    public class CategoryServiceModel
    {
        public List<int> SelectedSeatsIds { get; set; }

        public List<CategorySeatModel> Seats { get; set; }

        public List<int> CategoryIds { get; set; }

        public string Stage { get; set; }

        public string SelectedSeats { get; set; }
    }
}
