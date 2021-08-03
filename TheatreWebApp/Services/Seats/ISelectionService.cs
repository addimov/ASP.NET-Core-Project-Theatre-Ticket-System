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

        BookingFormModel GetSeatingChart(int showId, int currentPage = 1);

        BookingFormModel GetSeatingChart(BookingFormModel bookingChart);

    }
}
