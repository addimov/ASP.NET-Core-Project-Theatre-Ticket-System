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
    }
}
