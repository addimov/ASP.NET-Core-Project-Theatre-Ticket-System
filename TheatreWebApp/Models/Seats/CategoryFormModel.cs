using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Seats
{
    public class CategoryFormModel
    {
        public string SelectedSeats { get; set; }

        public IEnumerable<SeatCategoryViewModel> SeatsCategories { get; set; }

        [Required]
        public string CategoryNameInput { get; set; }

        [Required]
        public decimal PriceInput { get; set; }
    }
}
