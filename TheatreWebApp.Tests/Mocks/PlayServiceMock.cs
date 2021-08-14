using System.Collections.Generic;
using Moq;
using TheatreWebApp.Services.Plays;
using TheatreWebApp.Services.Plays.Models;

namespace TheatreWebApp.Tests.Mocks
{
    public class PlayServiceMock
    {
        public IPlayService Instance
        {
            get
            {
                var playServiceMock = new Mock<IPlayService>();

                playServiceMock
                    .Setup(s => s.All(null, 1, false, 6))
                    .Returns(new PlayQueryServiceModel
                    {
                        CurrentPage = 1,
                        PlaysPerPage = 6,
                        TotalPlays = 2,
                        SearchTerm = null,
                        ShowHidden = false,
                        Plays = new List<PlayServiceModel>
                        { new PlayServiceModel
                        {
                            Id = 1,
                            ImageUrl = "url",
                            Name = "Name",
                            ShortDescription = "Text"
                        },
                        new PlayServiceModel
                        {
                            Id = 2,
                            ImageUrl = "url",
                            Name = "Name",
                            ShortDescription = "Text"
                        }
                        }
                    });

                return playServiceMock.Object;
            }
        }

    }
}
