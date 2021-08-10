using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Stages;
using TheatreWebApp.Services.Categories;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Seats.Models;

namespace TheatreWebApp.Controllers
{
    public class StagesController : Controller
    {
        private readonly TheatreDbContext data;
        private readonly ISelectionService selection;
        private readonly ICategoryService category;

        public StagesController(TheatreDbContext data, ISelectionService selection, ICategoryService category)
        {
            this.data = data;
            this.selection = selection;
            this.category = category;
        }

        public IActionResult All()
        {
            var stages = data.Stages
                .Select(s => new StageListViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    SeatCount = s.Seats.Count()
                })
                .ToList();

            return View(stages);
        }

        public IActionResult Details(int stageId)
        {
            var stage = selection.StageDetails(stageId);

            return View(stage);
        }

        [HttpPost]
        public IActionResult Details(StageServiceModel stage)
        {
            stage = selection.StageDetails(stage.Id, stage.Name, stage.SelectedSeatId, stage.SelectedSeats, stage.CurrentPage);

            return View(stage);
        }

        public IActionResult Edit(string selectedSeats)
        {
            var categoryDataModel = category.GetViewData(selectedSeats);

            var categoryForm = category.GetFormModel(categoryDataModel);

            return View(categoryForm);
        }

        [HttpPost]
        public IActionResult Edit(CategoryFormModel categoryInput)
        {

            category.EditCategory(categoryInput);

            return RedirectToAction("Edit", new { categoryInput.SelectedSeats });
        }

    }
}
