using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats.Models;

namespace TheatreWebApp.Services.Seats
{
    public class SelectionService : ISelectionService
    {
        private readonly TheatreDbContext data;

        public SelectionService(TheatreDbContext data)
        {
            this.data = data;
        }

        //Stages --

        public IEnumerable<StageListServiceModel> All()
        {
            var stages = data.Stages
                .Select(s => new StageListServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    SeatCount = s.Seats.Count()
                })
                .ToList();

            return stages;
        }

        public StageServiceModel StageDetails(int stageId, int currentPage = 1)
        {
            var stage = data.Stages
                .Where(s => s.Id == stageId)
                .Select(s => new StageServiceModel
                {
                    Id = stageId,
                    Name = s.Name
                })
                .FirstOrDefault();

            var seatsQuery = GetSeats(stageId, currentPage);

            stage.Seats = seatsQuery
                .Select(s => new SeatServiceModel
                {
                    Id = s.Id,
                    Number = s.Number,
                    Price = s.SeatCategory.Price,
                    Row = s.Row
                })
                .ToList();

            stage.Rows = GetRows(seatsQuery);

            return stage;
        }

        public StageServiceModel StageDetails(
            int stageId,
            string name,
            int selectedSeatId = 0,
            string selectedSeats = null,
            int currentPage = 1)
        {
            var seatsQuery = GetSeats(stageId, currentPage);

            var selectedSeatsList = GetSelectedSeats(selectedSeats, selectedSeatId);

            var stage = new StageServiceModel
            {
                Id = stageId,
                Name = name,
                CurrentPage = currentPage
            };

            stage.Seats = seatsQuery
                .Select(s => new SeatServiceModel
                {
                    Id = s.Id,
                    Number = s.Number,
                    Price = s.SeatCategory.Price,
                    IsSelected = selectedSeatsList.Contains(s.Id) ? true : false,
                    Row = s.Row
                })
                .ToList();

            stage.SelectedSeats = string.Join(" ", selectedSeatsList);

            stage.Rows = GetRows(seatsQuery);

            return stage;
        }

        //Users --
        /*
        public BookingFormModel SeatingChart(int showId, int currentPage = 1, int selectedSeat = 0, List<int> selectedSeats = null)
        {
            var seatsQuery = PrepareSeatsQuery(showId, currentPage);
          
            var bookingChart = data.Shows
               .Where(s => s.Id == showId)
               .Select(s => new BookingFormModel
               {
                   ShowId = showId,
                   PlayName = s.Play.Name,
                   StageName = s.Stage.Name,
                   Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time),
               })
               .FirstOrDefault();

            bookingChart.SelectedSeats = GetSelectedSeats(selectedSeats, selectedSeat);

            bookingChart.Seats = PrepareBookingChart(bookingChart.SelectedSeats, showId, seatsQuery).ToList();

            bookingChart.Rows = GetRows(seatsQuery);

            return bookingChart;
        }
        */
        public BookingFormModel GetSeatingChart(int showId, int currentPage = 1)
        {
            var stageId = data.Shows
               .Where(s => s.Id == showId)
               .Select(s => s.StageId)
               .FirstOrDefault();

            var seatsQuery = PrepareSeatsQuery(showId);

            var bookingChart = data.Shows
                .Where(s => s.Id == showId)
                .Select(s => new BookingFormModel
                {
                    ShowId = showId,
                    PlayName = s.Play.Name,
                    StageName = s.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", s.Time),
                })
                .FirstOrDefault();

            bookingChart.Seats = PrepareBookingChart(null, showId, seatsQuery).ToList();

            bookingChart.Rows = GetRows(seatsQuery);

            return bookingChart;
        }

        public BookingFormModel GetSeatingChart(BookingFormModel bookingChart)
        {
            var seatsQuery = PrepareSeatsQuery(bookingChart.ShowId, bookingChart.CurrentPage);

            var selectedSeatsList = GetSelectedSeats(bookingChart.SelectedSeats, bookingChart.SelectedSeatId);

            bookingChart.SelectedSeats = string.Join(" ", selectedSeatsList);

            bookingChart.Seats = PrepareBookingChart(selectedSeatsList, bookingChart.ShowId, seatsQuery).ToList();

            bookingChart.Rows = GetRows(seatsQuery);

            return bookingChart;
        }

        public bool IsSeatTaken(int seatId, int showId)
        {
            var takenSeats = GetTakenSeats(showId);

            if (takenSeats.Contains(seatId))
            {
                return true;
            }

            return false;
        }

        //Common --

        private IQueryable<Seat> PrepareSeatsQuery(int showId, int currentPage = 1)
        {
            var stageId = data.Shows
               .Where(s => s.Id == showId)
               .Select(s => s.StageId)
               .FirstOrDefault();

            var seatsQuery = GetSeats(stageId, currentPage);

            return seatsQuery;
        }

        private List<int> GetSelectedSeats(string selectedSeats, int selectedSeatId)
        {
            var selectedSeatsList = new List<int>();

            if (selectedSeatId != 0)
            {
                var selectedSeat = data.Seats
                .Where(s => s.Id == selectedSeatId)
                .Select(s => s.Id)
                .FirstOrDefault();

                if (selectedSeats == null)
                {
                    selectedSeatsList.Add(selectedSeat);

                }
                else
                {
                    selectedSeatsList = selectedSeats.Split().Select(int.Parse).ToList();

                    if (selectedSeatsList.Contains(selectedSeat))
                    {
                        selectedSeatsList.Remove(selectedSeat);
                    }
                    else
                    {
                        selectedSeatsList.Add(selectedSeat);
                    }

                }
            }

            return selectedSeatsList;
        }

        private List<int> GetSelectedSeats(List<int> selectedSeats, int selectedSeatId)
        {
            if (selectedSeatId != 0)
            {
                var selectedSeat = data.Seats
                .Where(s => s.Id == selectedSeatId)
                .Select(s => s.Id)
                .FirstOrDefault();

                if (selectedSeats == null)
                {
                    selectedSeats = new List<int>();
                    selectedSeats.Add(selectedSeat);
                }
                else
                {

                    if (selectedSeats.Contains(selectedSeat))
                    {
                        selectedSeats.Remove(selectedSeat);
                    }
                    else
                    {
                        selectedSeats.Add(selectedSeat);
                    }

                }
            }

            return selectedSeats;
        }

        private List<int> GetRows(IQueryable<Seat> seatsQuery)
        {
            var rows = seatsQuery.Select(s => s.Row).Distinct().OrderBy(r => r).ToList();

            return rows;
        }

        private List<int?> GetTakenSeats(int showId)
        {
            var takenSeats = data.Reservations
                .Where(r => r.ShowId == showId)
                .Select(s => s.SeatId)
                .ToList();

            return takenSeats;
        }

        private IEnumerable<SeatServiceModel> PrepareBookingChart(List<int> selectedSeats, int showId, IQueryable<Seat> seatQuery)
        {
            var takenSeats = GetTakenSeats(showId);

            if (selectedSeats != null)
            {
                var seats = seatQuery
                    .Select(s => new SeatServiceModel
                    {
                        Id = s.Id,
                        Number = s.Number,
                        Row = s.Row,
                        Price = s.SeatCategory.Price,
                        IsSelected = selectedSeats.Contains(s.Id) ? true : false,
                        IsTaken = takenSeats.Contains(s.Id) ? true : false
                    })
                    .ToList();

                return seats;
            }
            else
            {
                var seats = seatQuery
                    .Select(s => new SeatServiceModel
                    {
                        Id = s.Id,
                        Number = s.Number,
                        Row = s.Row,
                        Price = s.SeatCategory.Price,
                        IsTaken = takenSeats.Contains(s.Id) ? true : false
                    })
                    .ToList();

                return seats;
            }
        }


        private IQueryable<Seat> GetSeats(int stageId, int currentPage)
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
