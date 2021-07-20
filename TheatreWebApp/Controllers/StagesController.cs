﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Data;
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

        public IActionResult Index()
        {
            var stages = data.Stages.Select(s => s).ToList();

            var stagesConfig = new StageConfigModel { Stages = stages };

            return View(stagesConfig);
        }

        [HttpPost]
        public IActionResult Index(StageConfigModel stageConfig)
        {
            var stages = data.Stages.Select(s => s).ToList();

            stageConfig.Stages = stages;

            var seatsQuery = data.Seats.Where(s => s.StageId == stageConfig.Id).Select(s => s).AsQueryable();

            if(stageConfig.SelectedSeatId == 0)
            {
                stageConfig.Seats = seatsQuery
                .Select(s => new SeatViewModel
                {
                    Id = s.Id,
                    Number = s.Number,
                    Row = s.Row
                })
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToList();
            }

            if (stageConfig.SelectedSeatId != 0)
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
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToList();

                stageConfig.SelectedSeatId = 0;
            }

           
            return View(stageConfig);
        }
    }
}
