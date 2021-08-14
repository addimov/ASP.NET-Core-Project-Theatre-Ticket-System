using System.Linq;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Services.Plays;
using TheatreWebApp.Tests.Mocks;
using Xunit;

namespace TheatreWebApp.Tests.Services
{
    public class PlayServiceTest
    {
        [Fact]
        public void AllShouldReturnAllVisiblePlaysOrderedByHighestId()
        {
            //Arrange
            var data = GetPlaysData();

            var playService = new PlayService(data);

            //Act
            var result = playService.All();
            var plays = result.Plays.ToList();

            //Assert
            Assert.NotNull(plays);
            Assert.Equal(2, plays.Count);
            Assert.Equal("Test2", plays[0].Name);
            Assert.Equal(3, plays[0].Id);
        }

        [Fact]

        public void AllShouldReturnAllPlaysOrderedByHighestId()
        {
            //Arrange
            var data = GetPlaysData();

            var playService = new PlayService(data);

            //Act
            var result = playService.All(null, 1, true);
            var plays = result.Plays.ToList();

            //Assert
            Assert.NotNull(plays);
            Assert.Equal(3, plays.Count);
            Assert.Equal("TestHidden", plays[2].Name);
        }


        private static TheatreDbContext GetPlaysData()
        {
            var data = DatabaseMock.Instance;

            data.Plays.Add(new Play
            {
                Id = 1,
                Name = "TestHidden",
                ShortDescription = "ShortText",
                Description = "LongText",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = true
            });
            data.Plays.Add(new Play
            {
                Id = 2,
                Name = "Test1",
                ShortDescription = "ShortText",
                Description = "LongText",
                Credits = "Credits",
                ImageUrl = "Url",
            });
            data.Plays.Add(new Play
            {
                Id = 3,
                Name = "Test2",
                ShortDescription = "ShortText",
                Description = "LongText",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });
            data.SaveChanges();

            return data;
        }

    }
}
