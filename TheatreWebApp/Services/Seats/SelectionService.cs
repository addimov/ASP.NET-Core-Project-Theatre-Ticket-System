using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Stages;

namespace TheatreWebApp.Services.Seats
{
    public class SelectionService : ISelectionService
    {
        private readonly TheatreDbContext data;

        public SelectionService(TheatreDbContext data)
        {
            this.data = data;
        }

        public SelectionServiceModel GetSelectedSeats(SelectionServiceModel input)
        {
            var seatsQuery = PrepareSeatingChart(input.Id, input.CurrentPage);

            input = SeatSelector(input, seatsQuery);

            return input;
        }

        private SelectionServiceModel SeatSelector(SelectionServiceModel input, IQueryable<Seat> seatsQuery)
        {
            var selectedSeat = data.Seats
                .Where(s => s.Id == input.SelectedSeatId)
                .Select(s => s.Id)
                .FirstOrDefault();

            var selectedSeatsList = new List<int>();

            if (input.SelectedSeats == null)
            {
                selectedSeatsList.Add(selectedSeat);

            }
            else
            {
                selectedSeatsList = input.SelectedSeats.Split().Select(int.Parse).ToList();

                if (selectedSeatsList.Contains(selectedSeat))
                {
                    selectedSeatsList.Remove(selectedSeat);
                }
                else
                {
                    selectedSeatsList.Add(selectedSeat);
                }
            }

            input.SelectedSeats = string.Join(" ", selectedSeatsList);

            input.Seats = seatsQuery
            .Select(s => new SeatViewModel
            {
                Id = s.Id,
                Number = s.Number,
                Row = s.Row,
                IsSelected = selectedSeatsList.Contains(s.Id) ? true : false
            })
            .ToList();

            input.SelectedSeatId = 0;

            return input;
        }

        public IQueryable<Seat> PrepareSeatingChart(int stageId, int currentPage)
        {
            var seatsQuery = data.Seats
                .Where(s => s.StageId == stageId)
                .Select(s => s)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .AsQueryable();

            if (stageId == 2)
            {
                currentPage = 1;
            }

            if (currentPage == 1)
            {
                seatsQuery = seatsQuery.Take(300);
            }
            else if (currentPage == 2)
            {
                seatsQuery = seatsQuery.Skip(300).Take(150);
            }
            else if (currentPage == 3)
            {
                seatsQuery = seatsQuery.Skip(450).Take(100);
            }

            return seatsQuery;
        }
    }
}
