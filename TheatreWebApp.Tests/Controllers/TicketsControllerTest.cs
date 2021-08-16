using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Controllers;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Seats;
using TheatreWebApp.Services.Shows;
using TheatreWebApp.Services.Tickets;
using TheatreWebApp.Services.Tickets.Model;
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

            var controller = PrepareController(data);


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

            var controller = PrepareController(data);

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

            var modelPost = viewResultPost.Model;

            var selectSeatsViewModelPost = Assert.IsType<BookingFormModel>(modelPost);

            Assert.Equal(28, int.Parse(selectSeatsViewModelPost.SelectedSeats));
        }

        [Fact]
        public void ReviewShouldReturnViewWithCorrectModelAndTicket()
        {
            //Arrange

            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows().GetSeatCategories().GetBigStageSeats();

            var controller = PrepareController(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestId")
            },
                "TestAuthentication"));

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var showId = 2;
            var selectedSeats = "28 34";

            //Act
            var result = controller.Review(showId, selectedSeats);

            //Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var ticketViewModel = Assert.IsType<TicketServiceModel>(model);

            Assert.Equal(2, ticketViewModel.ShowId);
            Assert.True(data.Tickets.Any(t => t.UserId == "TestId"));
        }

        [Fact]
        public void ReviewShouldReturnViewWithCorrectModelAndTicketActionOnConfirm()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows().GetSeatCategories().GetBigStageSeats();

            var controller = PrepareController(data);

            data.ReservationStatuses.Add(new ReservationStatus { Id = 1, Name = "Paid" });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestId")
            },
                "TestAuthentication"));

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var ticket = new Ticket
            {
                Id = "TestId",
                ShowId = 2,
                UserId = "TestId",
                CreatedOn = DateTime.Now,
                Reservations = new List<Reservation> { new Reservation { Id = "Test", SeatId = 28, ShowId = 2, Price = 20.00m } },
                ReservationStatusId = 1
            };

            data.Tickets.Add(ticket);

            data.SaveChanges();

            var ticketForm = new TicketFormModel { TicketId = "TestId", ShowId = 2, Action = 2 };

            //Act
            var result = controller.Review(ticketForm);

            //Assert
            Assert.NotNull(result);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.True(data.Tickets.Any(t => t.UserId == "TestId"));

            Assert.Equal("All", redirectResult.ActionName);
        }

        [Fact]
        public void ReviewShouldReturnViewWithCorrectModelAndTicketActionOnCancel()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows().GetSeatCategories().GetBigStageSeats();

            var controller = PrepareController(data);

            data.ReservationStatuses.Add(new ReservationStatus { Id = 1, Name = "Paid" });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestId")
            },
                "TestAuthentication"));

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var ticket = new Ticket
            {
                Id = "TestId",
                ShowId = 2,
                UserId = "TestId",
                CreatedOn = DateTime.Now,
                Reservations = new List<Reservation> { new Reservation { Id = "Test", SeatId = 28, ShowId = 2, Price = 20.00m } },
                ReservationStatusId = 1
            };

            data.Tickets.Add(ticket);

            data.SaveChanges();

            var ticketForm = new TicketFormModel { TicketId = "TestId", ShowId = 2, Action = 1 };

            //Act
            var result = controller.Review(ticketForm);

            //Assert
            Assert.NotNull(result);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.False(data.Tickets.Any(t => t.UserId == "TestId"));

            Assert.Equal("SelectSeats", redirectResult.ActionName);
        }

        private static TicketsController PrepareController(TheatreDbContext data)
        {
            var selectionService = new SelectionService(data);
            var ticketService = new TicketService(data);
            var showService = new ShowService(data);

            var controller = new TicketsController(selectionService, ticketService, showService);

            return controller;
        }
    }
}
