using System.Collections.Generic;
using System.Linq;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Seats;
using TheatreWebApp.Services.Categories.Models;

namespace TheatreWebApp.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly TheatreDbContext data;

        public CategoryService(TheatreDbContext data)
        {
            this.data = data;
        }

        public void EditCategory(CategoryFormModel categoryInput)
        {
            var seatIds = categoryInput.SelectedSeats.Split().Select(int.Parse).ToList();

            var seats = data.Seats
                .Where(s => seatIds.Contains(s.Id))
                .Select(s => s)
                .ToList();

            var category = data.SeatCategories
                .Where(c => c.Name == categoryInput.CategoryNameInput)
                .FirstOrDefault();

            if (category != null)
            {
                category.Price = categoryInput.PriceInput;

                data.SeatCategories.Update(category);

                foreach (var seat in seats)
                {
                    seat.SeatCategoryId = category.Id;
                }

                data.Seats.UpdateRange(seats);

            }
            else
            {
                category = new SeatCategory
                {
                    Name = categoryInput.CategoryNameInput,
                    Price = categoryInput.PriceInput,
                    Seats = seats
                };

                data.SeatCategories.Add(category);

            }

            data.SaveChanges();
        }

        public CategoryFormModel GetFormModel(CategoryServiceModel viewData)
        {
            var selectedCategories = new List<SeatCategoryViewModel>();

            var allCategories = data.SeatCategories
                .OrderBy(c => c.Id)
                .Select(c => new CategoryViewModel
                {
                    Name = c.Name,
                    Price = c.Price
                })
                .ToList();
            
            foreach (var category in viewData.CategoryIds)
            {
                var seatsInCategory = viewData.Seats
                    .Where(s => s.CategoryId == category)
                    .Select(s => s.SeatNumber)
                    .ToList();

                var seatsString = string.Join(", ", seatsInCategory);

                var categoryModel = data.SeatCategories
                    .Where(c => c.Id == category)
                    .Select(c => new SeatCategoryViewModel
                    {
                        CategoryName = c.Name,
                        Price = c.Price,
                        Seats = seatsString,
                        SeatsCount = seatsInCategory.Count(),
                        StageName = viewData.Stage
                    })
                    .FirstOrDefault();

                selectedCategories.Add(categoryModel);
            }

            var categoryForm = new CategoryFormModel
            {
                SelectedSeats = viewData.SelectedSeats,
                SeatsCategories = selectedCategories,
                AllCategories = allCategories
            };

            return categoryForm;
        }

        public CategoryServiceModel GetViewData(string selectedSeats)
        {
            var selectedSeatsIds = selectedSeats.Split().Select(int.Parse).ToList();

            var seats = data.Seats
                .Where(s => selectedSeatsIds.Contains(s.Id))
                .Select(s => new CategorySeatModel 
                {
                    SeatId = s.Id,
                    CategoryId = s.SeatCategoryId,
                    SeatNumber = s.Number
                })
                .ToList();

            var categoriesIds = data.Seats
                .Where(s => selectedSeatsIds.Contains(s.Id))
                .Select(s => s.SeatCategoryId)
                .Distinct()
                .ToList();

            var stage = data.Seats.Where(s => selectedSeatsIds.Contains(s.Id)).Select(s => s.Stage.Name).FirstOrDefault();

            var viewData = new CategoryServiceModel
            {
                SelectedSeatsIds = selectedSeatsIds,
                Seats = seats,
                CategoryIds = categoriesIds,
                Stage = stage,
                SelectedSeats = selectedSeats
            };

            return viewData;
        }
    }
}
