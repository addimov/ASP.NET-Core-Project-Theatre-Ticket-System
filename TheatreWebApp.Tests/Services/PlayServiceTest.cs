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
            var data = DatabaseMock.Instance.GetPlaysData();

            var playService = new PlayService(data);

            //Act
            var result = playService.All();
            var plays = result.Plays.ToList();

            //Assert
            Assert.NotNull(plays);
            Assert.Equal(6, plays.Count);
            Assert.Equal("Play Seven", plays[0].Name);
            Assert.Equal("Play Two", plays[5].Name);
        }

        [Fact]

        public void AllShouldReturnAllPlaysOrderedByHighestId()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlaysData();

            var playService = new PlayService(data);

            //Act
            var result = playService.All(null, 1, true);
            var plays = result.Plays.ToList();

            //Assert
            Assert.NotNull(plays);
            Assert.Equal(7, plays.Count);
            Assert.Equal("Play Seven", plays[0].Name);
            Assert.Equal("Play Hidden", plays[6].Name);
        }
    }
}
