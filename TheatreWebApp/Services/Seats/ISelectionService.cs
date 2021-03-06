using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Seats;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats.Models;

namespace TheatreWebApp.Services.Seats
{
    public interface ISelectionService
    {
        BookingFormModel GetSeatingChart(int showId, int currentPage = 1);

        BookingFormModel GetSeatingChart(BookingFormModel bookingChart);

        //BookingFormModel SeatingChart(int showId, int currentPage = 1, int selectedSeat = 0, List<int> SelectedSeats = null);

        StageServiceModel StageDetails(int stageId, int currentPage = 1);

        StageServiceModel StageDetails(
            int stageId,
            string name,
            int selectedSeatId = 0,
            string selectedSeats = null,
            int currentPage = 1
            );

        IEnumerable<StageListServiceModel> All();

        bool IsSeatTaken(int seatId, int showId);
    }
}
