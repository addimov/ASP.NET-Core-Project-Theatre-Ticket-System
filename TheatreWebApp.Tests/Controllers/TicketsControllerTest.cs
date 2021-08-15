using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Controllers;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Shows;
using TheatreWebApp.Services.Tickets;
using TheatreWebApp.Tests.Mocks;
using Xunit;

namespace TheatreWebApp.Tests.Controllers
{
    public class TicketsControllerTest
    {

        [Fact]
        public void SelectSeatsShouldReturnViewWithCorrectModel()
        {
            //Arramge
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows().GetSeatCategories().GetBigStageSeats();

            var selectionService = new SelectionService(data);
            var ticketService = new TicketService(data);
            var showService = new ShowService(data);

            var controller = new TicketsController(selectionService, ticketService, showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var showId = 2;

            //Act

            var result = controller.SelectSeats(showId);

            //Assert

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var selectSeatsViewModel = Assert.IsType<BookingFormModel>(model);

            Assert.Equal(300, selectSeatsViewModel.Seats.Count());
        }

        [Fact]
        public void SelectSeatsPostShouldReturnViewWithCorrectSelection()
        {
            //Arramge
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows().GetSeatCategories().GetBigStageSeats();

            var selectionService = new SelectionService(data);
            var ticketService = new TicketService(data);
            var showService = new ShowService(data);

            var controller = new TicketsController(selectionService, ticketService, showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var showId = 2;


            //Act

            var resultGet = controller.SelectSeats(showId);


            //Assert

            Assert.NotNull(resultGet);

            var viewResult = Assert.IsType<ViewResult>(resultGet);

            var model = viewResult.Model;

            var selectSeatsViewModel = Assert.IsType<BookingFormModel>(model);

            var form = new BookingFormModel
            {
                ShowId = selectSeatsViewModel.ShowId,
                PlayName = selectSeatsViewModel.PlayName,
                StageName = selectSeatsViewModel.StageName,
                SelectedSeatId = 28,
                Time = selectSeatsViewModel.Time
            };

            var resultPost = controller.SelectSeats(form);

            var viewResultPost = Assert.IsType<ViewResult>(resultPost);

            var modelPost = viewResult.Model;

            var selectSeatsViewModelPost = Assert.IsType<BookingFormModel>(modelPost);

            Assert.Equal(28, int.Parse(selectSeatsViewModelPost.SelectedSeats));

        }
    }
}
