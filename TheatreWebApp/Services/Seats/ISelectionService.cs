using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Services.Seats
{
    public interface ISelectionService
    {
        public SelectionServiceModel GetSelectedSeats(SelectionServiceModel input);

        IQueryable<Seat> PrepareSeatingChart(int stageId, int currentPage);
    }
}
