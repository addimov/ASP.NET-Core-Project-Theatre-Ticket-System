using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheatreWebApp.Controllers;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Plays;
using TheatreWebApp.Services.Plays;
using TheatreWebApp.Services.Plays.Models;
using TheatreWebApp.Tests.Mocks;
using Xunit;

namespace TheatreWebApp.Tests.Controllers
{
    public class PlaysControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectViewModel()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlaysData();

            var playService = new PlayService(data);

            var controller = new PlaysController(playService);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var query = new PlayQueryModel();

            //Act
            var result = controller.All(query);

            //Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var playsAllViewModel = Assert.IsType<PlayQueryModel>(model);

            Assert.Equal(6, playsAllViewModel.TotalPlays);
        }

        [Fact]
        public void AllShouldReturnViewWithCorrectViewModelbySearchTerm()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlaysData();

            var playService = new PlayService(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var controller = new PlaysController(playService);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var query = new PlayQueryModel { SearchTerm = "Six"};

            //Act
            var result = controller.All(query);

            //Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var playsAllViewModel = Assert.IsType<PlayQueryModel>(model);

            var plays = Assert.IsAssignableFrom<IEnumerable<PlayServiceModel>>(playsAllViewModel.Plays).ToList();

            Assert.Equal(1, playsAllViewModel.TotalPlays);

            Assert.Equal("Play Six", plays[0].Name);

        }
    }
}
