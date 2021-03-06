using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Models.Seats;
using TheatreWebApp.Services.Categories;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Seats.Models;

namespace TheatreWebApp.Areas.Admin.Controllers
{
    public class SeatsController : AdminController
    {
        private readonly ISelectionService selection;
        private readonly ICategoryService category;

        public SeatsController(ISelectionService selection, ICategoryService category)
        {
            this.selection = selection;
            this.category = category;
        }

        public IActionResult All()
        {
            var stages = this.selection.All();

            return View(stages);
        }

        public IActionResult Details(int stageId)
        {
            var stage = this.selection.StageDetails(stageId);

            return View(stage);
        }

        [HttpPost]
        public IActionResult Details(StageServiceModel stage)
        {
            stage = this.selection.StageDetails(stage.Id, stage.Name, stage.SelectedSeatId, stage.SelectedSeats, stage.CurrentPage);

            return View(stage);
        }

        public IActionResult Edit(string selectedSeats)
        {
            var categoryDataModel = this.category.GetViewData(selectedSeats);

            var categoryForm = this.category.GetFormModel(categoryDataModel);

            return View(categoryForm);
        }

        [HttpPost]
        public IActionResult Edit(CategoryFormModel categoryInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            this.category.EditCategory(categoryInput);

            return RedirectToAction("Edit", new { categoryInput.SelectedSeats });
        }
    }
}
