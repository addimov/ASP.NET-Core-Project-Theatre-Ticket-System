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
            var data = GetPlaysData();

            var playService = new PlayService(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var controller = new PlaysController(playService);

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
            var data = GetPlaysData();

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

            Assert.Equal("Test Six", plays[0].Name);

        }


        private static TheatreDbContext GetPlaysData()
        {
            var data = DatabaseMock.Instance;

            data.Plays.Add(new Play
            {
                Id = 1,
                Name = "TestHidden",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = true
            });
            data.Plays.Add(new Play
            {
                Id = 2,
                Name = "Test1",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
            });
            data.Plays.Add(new Play
            {
                Id = 3,
                Name = "Test2",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });
            data.Plays.Add(new Play
            {
                Id = 4,
                Name = "Test Four",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });

            data.Plays.Add(new Play
            {
                Id = 5,
                Name = "Test Five",
                ShortDescription = "PlaceholderText",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });
            data.Plays.Add(new Play
            {
                Id = 6,
                Name = "Test Six",
                ShortDescription = "PlaceholderText",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });

            data.Plays.Add(new Play
            {
                Id = 7,
                Name = "Test Seven",
                ShortDescription = "PlaceholderText",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });


            data.SaveChanges();

            return data;
        }

    }
}
