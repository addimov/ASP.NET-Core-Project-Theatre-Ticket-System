using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Stages;

namespace TheatreWebApp.Services.Categories
{
    public interface ICategoryService
    {
        public CategoryServiceModel GetViewData(string selectedSeats);

        public CategoryFormModel GetFormModel(CategoryServiceModel viewData);

        public void EditCategory(CategoryFormModel categoryForm);
    }
}
