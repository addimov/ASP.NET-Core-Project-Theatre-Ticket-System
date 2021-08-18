using TheatreWebApp.Models.Seats;
using TheatreWebApp.Services.Categories.Models;

namespace TheatreWebApp.Services.Categories
{
    public interface ICategoryService
    {
        public CategoryServiceModel GetViewData(string selectedSeats);

        public CategoryFormModel GetFormModel(CategoryServiceModel viewData);

        public void EditCategory(CategoryFormModel categoryForm);
    }
}
