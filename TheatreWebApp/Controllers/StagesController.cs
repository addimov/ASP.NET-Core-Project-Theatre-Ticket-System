using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Stages;

namespace TheatreWebApp.Controllers
{
    public class StagesController : Controller
    {
        private readonly TheatreDbContext data;

        public StagesController(TheatreDbContext data)
        {
            this.data = data;
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
            var stageQuery = data.Stages
                .Where(s => s.Id == stageId)
                .Select(s => new StageQueryModel
                {
                    Id = stageId,
                    Name = s.Name,
                })
                .FirstOrDefault();

            var seatsQuery = data.Seats
                .Where(s => s.StageId == stageId)
                .Select(s => s)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .AsQueryable();

            if (stageId == 2)
            {
                stageQuery.CurrentPage = 1;
            }

            if (stageQuery.CurrentPage == 1)
            {
                seatsQuery = seatsQuery.Take(300);
            }
            else if (stageQuery.CurrentPage == 2)
            {
                seatsQuery = seatsQuery.Skip(300).Take(150);
            }
            else if (stageQuery.CurrentPage == 3)
            {
                seatsQuery = seatsQuery.Skip(450).Take(100);
            }

            stageQuery.Seats = seatsQuery
                .Select(s => new SeatViewModel
                { 
                    Id = s.Id,
                    Number = s.Number,
                    Row = s.Row
                 })
                .ToList();

            return View(stageQuery);
        }

        [HttpPost]
        public IActionResult Details(StageQueryModel stageQuery)
        {
            var seatsQuery = data.Seats
                .Where(s => s.StageId == stageQuery.Id)
                .Select(s => s)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .AsQueryable();

            if (stageQuery.Id == 2)
            {
                stageQuery.CurrentPage = 1;
            }

            if (stageQuery.CurrentPage == 1)
            {
                seatsQuery = seatsQuery.Take(300);
            }
            else if (stageQuery.CurrentPage == 2)
            {
                seatsQuery = seatsQuery.Skip(300).Take(150);
            }
            else if (stageQuery.CurrentPage == 3)
            {
                seatsQuery = seatsQuery.Skip(450).Take(100);
            }

            stageQuery = SeatSelector(stageQuery, seatsQuery);

            return View(stageQuery);
        }

        public IActionResult Edit(string selectedSeats)
        {
            var selectedSeatsIds = selectedSeats.Split().Select(int.Parse).ToList();

            var seats = data.Seats
                .Where(s => selectedSeatsIds.Contains(s.Id))
                .Select(s => s)
                .ToList();

            var categoriesIds = data.Seats
                .Where(s => selectedSeatsIds.Contains(s.Id))
                .Select(s => s.SeatCategoryId)
                .Distinct()
                .ToList();

            var stage = data.Seats.Where(s => selectedSeatsIds.Contains(s.Id)).Select(s => s.Stage.Name).FirstOrDefault();

            var categories = new List<SeatCategoryViewModel>();

            foreach (var category in categoriesIds)
            {
                var seatsInCategory = seats
                    .Where(s => s.SeatCategoryId == category)
                    .Select(s => s.Number)
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
                        StageName = stage
                    })
                    .FirstOrDefault();

                categories.Add(categoryModel);
            }

            return View(categories);
        }

        private StageQueryModel SeatSelector(StageQueryModel stageQuery, IQueryable<Seat> seatsQuery)
        {
            var selectedSeat = data.Seats
                .Where(s => s.Id == stageQuery.SelectedSeatId)
                .Select(s => s.Id)
                .FirstOrDefault();

            var selectedSeatsList = new List<int>();

            if (stageQuery.SelectedSeats == null)
            {
                selectedSeatsList.Add(selectedSeat);
      
            }
            else
            {            
                selectedSeatsList = stageQuery.SelectedSeats.Split().Select(int.Parse).ToList();

                if (selectedSeatsList.Contains(selectedSeat))
                {
                    selectedSeatsList.Remove(selectedSeat);
                }
                else
                {
                    selectedSeatsList.Add(selectedSeat);
                }              
            }

            stageQuery.SelectedSeats = string.Join(" ", selectedSeatsList);

            stageQuery.Seats = seatsQuery
            .Select(s => new SeatViewModel
            {
                Id = s.Id,
                Number = s.Number,
                Row = s.Row,
                IsSelected = selectedSeatsList.Contains(s.Id) ? true : false
            })
            .ToList();

            stageQuery.SelectedSeatId = 0;

            return stageQuery;
        }
    }
}
