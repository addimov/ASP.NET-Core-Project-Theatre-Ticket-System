using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Controllers;
using TheatreWebApp.Models.Shows;
using TheatreWebApp.Services.Shows;
using TheatreWebApp.Tests.Mocks;
using Xunit;

namespace TheatreWebApp.Tests.Controllers
{
    public class ShowsControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectModel()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var controller = new ShowsController(showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var query = new ShowQueryModel();

            //Act

            var result = controller.All(query);

            //Assert

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var showAllViewModel = Assert.IsType<ShowQueryModel>(model);

            var shows = showAllViewModel.Shows.ToList();

            Assert.Equal(4, showAllViewModel.Shows.Count());
            Assert.Equal(5, shows[0].Id);
        }

        [Fact]
        public void AllShouldReturnViewWithCorrectModelWhenSearchingByPlay()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var controller = new ShowsController(showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var query = new ShowQueryModel { PlayId = 3};

            //Act

            var result = controller.All(query);

            //Assert

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var showAllViewModel = Assert.IsType<ShowQueryModel>(model);

            var shows = showAllViewModel.Shows.ToList();

            Assert.Single(showAllViewModel.Shows);
            Assert.Equal(4, shows[0].Id);
        }

        [Fact]
        public void AllShouldReturnViewWithCorrectModelWhenSearchingByTerm()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var controller = new ShowsController(showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var query = new ShowQueryModel { SearchTerm = "Two" };

            //Act

            var result = controller.All(query);

            //Assert

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var showAllViewModel = Assert.IsType<ShowQueryModel>(model);

            var shows = showAllViewModel.Shows.ToList();

            Assert.Equal(2, showAllViewModel.Shows.Count());
            Assert.Equal("Play Two", shows[0].PlayName);
            Assert.Equal("Play Two", shows[0].PlayName);
        }

        [Fact]
        public void AllShouldReturnViewWithCorrectModelWhenSearchingByAfterDate()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var controller = new ShowsController(showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var startingDate = DateTime.Parse("14/08/2021 19:00");
            var currentDate = DateTime.UtcNow;
            var difference = currentDate - startingDate;
            var date = startingDate.AddDays(difference.Days);

            var dateString = date.AddDays(12).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query = new ShowQueryModel { AfterDate = dateString };

            //Act

            var result = controller.All(query);

            //Assert

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var showAllViewModel = Assert.IsType<ShowQueryModel>(model);

            var shows = showAllViewModel.Shows.ToList();

            Assert.Equal(2, showAllViewModel.Shows.Count());
            Assert.Equal("Play Two", shows[0].PlayName);
            Assert.Equal("Play Three", shows[1].PlayName);
        }

        [Fact]
        public void AllShouldReturnViewWithCorrectModelWhenSearchingByBeforeDate()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var controller = new ShowsController(showService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var startingDate = DateTime.Parse("14/08/2021 19:00");
            var currentDate = DateTime.UtcNow;
            var difference = currentDate - startingDate;
            var date = startingDate.AddDays(difference.Days);

            var dateString = date.AddDays(12).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query = new ShowQueryModel { BeforeDate = dateString };

            //Act

            var result = controller.All(query);

            //Assert

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var showAllViewModel = Assert.IsType<ShowQueryModel>(model);

            var shows = showAllViewModel.Shows.ToList();

            Assert.Equal(2, showAllViewModel.Shows.Count());
            Assert.Equal("Play Four", shows[0].PlayName);
            Assert.Equal("Play Two", shows[1].PlayName);
        }

    }
}
