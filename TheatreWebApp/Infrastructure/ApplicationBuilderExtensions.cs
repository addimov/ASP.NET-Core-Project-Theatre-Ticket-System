using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<TheatreDbContext>();

            data.Database.EnsureDeleted();
            data.Database.EnsureCreated();


            SeedStages(data);

            SeedSeatCategories(data);

            SeedSeats(data);

            SeedReservationStatuses(data);

            return app;
        }

        private static void SeedStages(TheatreDbContext data)
        {
            if (data.Stages.Any())
            {
                return;
            }

            data.Stages.AddRange(new[]
            {
                new Stage { Name = "Big stage", MaxSeats = 550 },
                new Stage { Name = "Small stage", MaxSeats = 60 },
            });

            data.SaveChanges();

        }

        private static void SeedSeatCategories(TheatreDbContext data)
        {
            if (data.SeatCategories.Any())
            {
                return;
            }

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

        }

        private static void SeedSeats(TheatreDbContext data)
        {
            if (data.Seats.Any())
            {
                return;
            }
           

            var seatsBigStage = PrepareSeatsBigStage(data);
            var seatsSmallStage = PrepareSeatsSmallStage(data);

            data.AddRange(seatsBigStage);
            data.AddRange(seatsSmallStage);
            data.SaveChanges();

        }

        private static void SeedReservationStatuses(TheatreDbContext data)
        {
            if (data.ReservationStatuses.Any())
            {
                return;
            }

            data.ReservationStatuses.AddRange(new[]
            {
                new ReservationStatus{ Name = "Confirmed" },
                new ReservationStatus { Name = "Paid" },
                new ReservationStatus { Name = "Expired" },
                new ReservationStatus { Name = "Unconfirmed" },
            });

            data.SaveChanges();
        }

        private static List<Seat> PrepareSeatsBigStage(TheatreDbContext data)
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


            return seatsBigStage;
        }

        private static List<Seat> PrepareSeatsSmallStage(TheatreDbContext data)
        {
            int maxSeatsSmall = data.Stages.Where(s => s.Name == "Small stage").Select(s => s.MaxSeats).FirstOrDefault();

            var seats = new List<Seat>();

            var rowCount = 1;

            for(int i = 1; i < maxSeatsSmall + 1; i++)
            {
                if(i/rowCount/15 == 1)
                {
                    rowCount++;
                }

                var seat = new Seat
                {
                    Number = i,
                    SeatCategoryId = data.SeatCategories.Where(c => c.Name == "SmallStage").Select(c => c.Id).FirstOrDefault(),
                    StageId = data.Stages.Where(s => s.Name == "Small stage").Select(s => s.Id).FirstOrDefault(),
                    Row = rowCount
                };

                seats.Add(seat);
            }

            return seats;
        }
    }
}