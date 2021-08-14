using System;
using Microsoft.EntityFrameworkCore;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Tests.Mocks
{
    public static class DatabaseMock
    {
        public static TheatreDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<TheatreDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new TheatreDbContext(dbContextOptions);
            }
        }

        public static TheatreDbContext GetPlaysData(this TheatreDbContext data)
        {
            data.Plays.Add(new Play
            {
                Id = 1,
                Name = "Play Hidden",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = true
            });
            data.Plays.Add(new Play
            {
                Id = 2,
                Name = "Play Two",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
            });
            data.Plays.Add(new Play
            {
                Id = 3,
                Name = "Play Three",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });
            data.Plays.Add(new Play
            {
                Id = 4,
                Name = "Play Four",
                ShortDescription = "Short Text",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });

            data.Plays.Add(new Play
            {
                Id = 5,
                Name = "Play Five",
                ShortDescription = "PlaceholderText",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });
            data.Plays.Add(new Play
            {
                Id = 6,
                Name = "Play Six",
                ShortDescription = "PlaceholderText",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });

            data.Plays.Add(new Play
            {
                Id = 7,
                Name = "Play Seven",
                ShortDescription = "PlaceholderText",
                Description = "Long Text",
                Credits = "Credits",
                ImageUrl = "Url",
                IsHidden = false
            });


            data.SaveChanges();

            return data;
        }

        public static TheatreDbContext GetStagesData(this TheatreDbContext data)
        {
            data.Stages.Add(new Stage
            {
                Id = 1,
                Name = "Big stage",
                MaxSeats = 550
            });

            data.SaveChanges();

            return data;
        }

        public static TheatreDbContext GetShowsData(this TheatreDbContext data)
        {
            var startingDate = DateTime.Parse("14/08/2021 19:00");
            var currentDate = DateTime.UtcNow;
            var difference = currentDate - startingDate;
            var date = startingDate.AddDays(difference.Days);

            data.Shows.Add(new Show
            {
                Id = 1,
                PlayId = 2,
                StageId = 1,
                Time = date.AddDays(2)         
            });
            data.Shows.Add(new Show
            {
                Id = 2,
                PlayId = 2,
                StageId = 1,
                Time = date.AddDays(15)
            });
            data.Shows.Add(new Show
            {
                Id = 3,
                PlayId = 3,
                StageId = 1,
                Time = date.AddDays(-10)
            });
            data.Shows.Add(new Show
            {
                Id = 4,
                PlayId = 3,
                StageId = 1,
                Time = date.AddDays(25)
            });
            data.Shows.Add(new Show
            {
                Id = 5,
                PlayId = 4,
                StageId = 1,
                Time = date
            });

            data.SaveChanges();

            return data;
        }
    }

}
