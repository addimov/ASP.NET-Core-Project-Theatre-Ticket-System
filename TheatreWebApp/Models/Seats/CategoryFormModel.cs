using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Models.Seats
{
    public class CategoryFormModel
    {
        public string SelectedSeats { get; set; }

        public IEnumerable<CategoryViewModel> AllCategories { get; set; }

        public IEnumerable<SeatCategoryViewModel> SeatsCategories { get; set; }

        [Required]
        [Display(Name = "Category name")]
        public string CategoryNameInput { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal PriceInput { get; set; }
    }
}
