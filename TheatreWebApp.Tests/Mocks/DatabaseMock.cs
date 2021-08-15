using System;
using System.Collections.Generic;
using System.Linq;
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

        public static TheatreDbContext GetPlays(this TheatreDbContext data)
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

        public static TheatreDbContext GetStages(this TheatreDbContext data)
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

        public static TheatreDbContext GetShows(this TheatreDbContext data)
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

        public static TheatreDbContext GetSeatCategories(this TheatreDbContext data)
        {
            data.SeatCategories.AddRange(new[]
            {
                new SeatCategory { Name = "Stalls_Front", Price = 40.00m },
                new SeatCategory { Name = "Stalls_Mid", Price = 30.00m },
                new SeatCategory { Name = "Stalls_Back", Price = 20.00m},
                new SeatCategory { Name = "Gallery_One_Front", Price = 40.00m },
                new SeatCategory { Name = "Gallery_One_Back", Price = 20.00m },
                new SeatCategory { Name = "Gallery_Two_Front", Price = 30.00m },
                new SeatCategory { Name = "Gallery_Two_Back", Price = 10.00m },
                new SeatCategory { Name = "SmallStage", Price = 50.00m }
            });

            data.SaveChanges();

            return data;
        }

        public static TheatreDbContext GetBigStageSeats(this TheatreDbContext data)
        {
            int maxSeatsBig = data.Stages.Where(s => s.Name == "Big stage").Select(s => s.MaxSeats).FirstOrDefault();

            //Stalls seats - 300, galleries - 150 and 100

            var seatsBigStage = new List<Seat>();
            var rowCount = 1;
            var seatNumEven = 2;
            var seatNumOdd = 1;
            var tempSeatCount = 0;
            var tempRowCount = 0;

            for (int i = 1; i < maxSeatsBig + 1; i++)
            {
                var seat = new Seat();

                if (rowCount % 2 == 0)
                {
                    seat.Number = seatNumEven;
                    seatNumEven += 2;
                }
                else
                {
                    seat.Number = seatNumOdd;
                    seatNumOdd += 2;
                }

                if (rowCount < 4)
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Stalls_Front").Select(c => c.Id).FirstOrDefault();
                }
                else if (rowCount < 13)
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Stalls_Mid").Select(c => c.Id).FirstOrDefault();
                }
                else if (rowCount < 16)
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Stalls_Back").Select(c => c.Id).FirstOrDefault();
                }
                else if (rowCount < 18)
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Gallery_One_Front").Select(c => c.Id).FirstOrDefault();
                }
                else if (rowCount < 21)
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Gallery_One_Back").Select(c => c.Id).FirstOrDefault();
                }
                else if (rowCount < 23)
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Gallery_Two_Front").Select(c => c.Id).FirstOrDefault();
                }
                else
                {
                    seat.SeatCategoryId = data.SeatCategories.Where(c => c.Name == "Gallery_Two_Back").Select(c => c.Id).FirstOrDefault();
                }


                seat.Row = rowCount;

                seat.StageId = data.Stages.Where(s => s.Name == "Big stage").Select(s => s.Id).FirstOrDefault();

                seatsBigStage.Add(seat);


                if (rowCount < 16 && seatsBigStage.Count() / rowCount / 20 == 1)
                {
                    rowCount++;
                    if (rowCount == 16)
                    {
                        tempRowCount = 1;
                        tempSeatCount = 300;
                    }
                }
                else if (rowCount > 15 && rowCount < 21 && (seatsBigStage.Count() - tempSeatCount) / tempRowCount / 30 == 1)
                {
                    rowCount++;
                    tempRowCount++;
                    if (rowCount == 21)
                    {
                        tempRowCount = 1;
                        tempSeatCount = 450;
                    }
                }
                else if (rowCount > 20 && (seatsBigStage.Count() - tempSeatCount) / tempRowCount / 25 == 1)
                {
                    rowCount++;
                    tempRowCount++;
                }
            }

            data.AddRange(seatsBigStage);
            data.SaveChanges();

            return data;
        }
    }

}
