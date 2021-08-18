using System.Collections.Generic;

namespace TheatreWebApp.Services.Categories.Models
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
