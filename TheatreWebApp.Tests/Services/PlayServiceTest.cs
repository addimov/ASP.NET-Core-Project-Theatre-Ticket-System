using System.Linq;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Services.Plays;
using TheatreWebApp.Services.Plays.Models;
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
            var data = DatabaseMock.Instance.GetPlays();

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
            var data = DatabaseMock.Instance.GetPlays();

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

        [Fact]
        public void AllShouldReturnAllPlaysBySearchTerm()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays();

            var playService = new PlayService(data);

            var searchTerm = "Long";

            //Act
            var result = playService.All(searchTerm, 1, false);
            var plays = result.Plays.ToList();

            //Assert
            Assert.NotNull(plays);
            Assert.Equal(6, plays.Count);
            Assert.Equal("Play Seven", plays[0].Name);
        }

        [Fact]
        public void DetailsShouldReturnCorrectModel()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays();

            var playService = new PlayService(data);

            var playId = 4;

            //Act
            var result = playService.Details(playId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<PlayDetailsServiceModel>(result);
            Assert.Equal("Play Four", result.Name);
            Assert.NotNull(result.Paragraphs);
            Assert.Equal(data.Plays.Where(p => p.Id == playId).Select(p => p.Description).FirstOrDefault(), result.Paragraphs.ToString());
        }

        [Fact]
        public void AllShouldReturnAllPlaysBySearchTermWithPlaysPerPage()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays();

            var playService = new PlayService(data);

            var searchTerm = "Long";
            var playsPerPage = 2;

            //Act
            var result = playService.All(searchTerm, 1, false, playsPerPage);
            var plays = result.Plays.ToList();

            //Assert
            Assert.NotNull(plays);
            Assert.Equal(2, plays.Count);
            Assert.Equal("Play Seven", plays[0].Name);
        }

    }
}
