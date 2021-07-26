using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Stages;
using TheatreWebApp.Models.Tickets;

namespace TheatreWebApp.Services.Seats
{
    public interface ISelectionService
    {
        SelectionServiceModel GetSelectedSeats(SelectionServiceModel input);

        IQueryable<Seat> PrepareSeatingChart(int stageId, int currentPage);

        IQueryable<Seat> PrepareSeatsQuery(int showId, int currentPage = 1);

        List<int> GetSelectedSeats(string selectedSeats, int selectedSeatId);

        IEnumerable<SeatViewModel> PrepareBookingChart(List<int> selectedSeats, int showId, IQueryable<Seat> seatQuery);


    }
}
