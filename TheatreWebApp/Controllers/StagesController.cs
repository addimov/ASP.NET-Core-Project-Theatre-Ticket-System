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

        public IActionResult Index(int id, int currentPage)
        {
            var stages = data.Stages.Select(s => s).ToList();

            var stagesConfig = new StageConfigModel { Stages = stages, Id = id, CurrentPage = currentPage };

            var seatsQuery = data.Seats
               .Where(s => s.StageId == stagesConfig.Id)
               .Select(s => s)
               .OrderBy(s => s.Row)
               .ThenBy(s => s.Number)
               .AsQueryable();

            if (stagesConfig.CurrentPage == 1)
            {
                seatsQuery = seatsQuery.Take(300);
            }
            else if (stagesConfig.CurrentPage == 2)
            {
                seatsQuery = seatsQuery.Skip(300).Take(150);
            }
            else if (stagesConfig.CurrentPage == 3)
            {
                seatsQuery = seatsQuery.Skip(450).Take(100);
            }

            stagesConfig.Seats = seatsQuery
                .Select(s => new SeatViewModel
                {
                    Id = s.Id,
                    Number = s.Number,
                    Row = s.Row
                })
                .ToList();

            return View(stagesConfig);
        }

        [HttpPost]
        public IActionResult Index(StageConfigModel stageConfig)
        {
            var stages = data.Stages.Select(s => s).ToList();

            stageConfig.Stages = stages;
            
            var seatsQuery = data.Seats
                .Where(s => s.StageId == stageConfig.Id)
                .Select(s => s)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .AsQueryable();
            
            if (stageConfig.CurrentPage == 1)
            {
                seatsQuery = seatsQuery.Take(300);
            } 
            else if(stageConfig.CurrentPage == 2)
            {
                seatsQuery = seatsQuery.Skip(300).Take(150);
            } 
            else if(stageConfig.CurrentPage == 3)
            {
                seatsQuery = seatsQuery.Skip(450).Take(100);
            }

      

            if (stageConfig.SelectedSeatId == 0)
            {
                stageConfig.Seats = seatsQuery
                .Select(s => new SeatViewModel
                {
                    Id = s.Id,
                    Number = s.Number,
                    Row = s.Row
                })
                .ToList();
            }

            if (stageConfig.SelectedSeatId != 0)
            {
                stageConfig = SeatSelector(stageConfig, seatsQuery);
            }

           
            return View(stageConfig);
        }

        private StageConfigModel SeatSelector(StageConfigModel stageConfig, IQueryable<Seat> seatsQuery)
        {
            var selectedSeat = data.Seats.Where(s => s.Id == stageConfig.SelectedSeatId).Select(s => s.Id).FirstOrDefault();

            var selectedSeatsList = new List<int>();

            if (stageConfig.SelectedSeats == null)
            {
                stageConfig.SelectedSeats = string.Join(" ", selectedSeat);
                selectedSeatsList = stageConfig.SelectedSeats.Split().Select(int.Parse).ToList();
            }
            else
            {
                selectedSeatsList = stageConfig.SelectedSeats.Split().Select(int.Parse).ToList();

                if (selectedSeatsList.Contains(selectedSeat))
                {
                    selectedSeatsList.Remove(selectedSeat);
                    stageConfig.SelectedSeats = string.Join(" ", selectedSeatsList);
                }
                else
                {
                    selectedSeatsList.Add(selectedSeat);
                    stageConfig.SelectedSeats = stageConfig.SelectedSeats + " " + selectedSeat.ToString();
                }
            }


            stageConfig.Seats = seatsQuery
            .Select(s => new SeatViewModel
            {
                Id = s.Id,
                Number = s.Number,
                Row = s.Row,
                IsSelected = selectedSeatsList.Contains(s.Id) ? true : false
            })
            .ToList();

            stageConfig.SelectedSeatId = 0;

            return stageConfig;
        }
    }
}
