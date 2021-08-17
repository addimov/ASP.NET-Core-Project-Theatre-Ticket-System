using System;
using System.Globalization;
using System.Linq;
using TheatreWebApp.Services.Shows;
using TheatreWebApp.Tests.Mocks;
using Xunit;

namespace TheatreWebApp.Tests.Services
{
    public class ShowServiceTest
    {
        [Fact]
        public void AllShouldReturnAllVisibleShowsOrderedByDate()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            //Act
            var result = showService.All();
            var shows = result.Shows.ToList();

            //Assert
            Assert.NotNull(shows);
            Assert.Equal(4, shows.Count());
            Assert.Equal(5, shows[0].Id);
            Assert.Equal(4, shows[3].Id);
        }

        [Fact]
        public void AllShouldReturnAllVisibleShowsBySearchTerm()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var searchTerm = "PlaceholderText";

            //Act
            var result = showService.All(0, searchTerm);
            var shows = result.Shows.ToList();

            //Assert
            Assert.Empty(shows);
        }

        [Fact]
        public void AllShouldReturnAllVisibleShowsByPlayAndBeforerDate()
        {
            //Arrange
            var data = DatabaseMock.Instance.GetPlays().GetStages().GetShows();

            var showService = new ShowService(data);

            var playId = 2;

            var startingDate = DateTime.Parse("14/08/2021 19:00");
            var currentDate = DateTime.UtcNow;
            var difference = currentDate - startingDate;
            var date = startingDate.AddDays(difference.Days);
            var beforeDate = date.AddDays(5).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            //Act
            var result = showService.All(playId, null, null, beforeDate);
            var shows = result.Shows.ToList();

            //Assert
            Assert.Single(shows);
            Assert.Equal(playId, shows[0].PlayId);
            Assert.Equal(1, shows[0].Id);
        }
    }
}
