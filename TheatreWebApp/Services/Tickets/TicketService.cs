using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TheatreWebApp.Data;
using TheatreWebApp.Data.Models;
using TheatreWebApp.Models.Tickets;
using TheatreWebApp.Services.Tickets.Model;

namespace TheatreWebApp.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly TheatreDbContext data;

        public TicketService(TheatreDbContext data)
        {
            this.data = data;
        }

        public TicketQueryModel AllByUser(
            string userId, 
            int currentPage = 1, 
            int ticketsPerPage = int.MaxValue,
            bool showAll = false)
        {
            var ticketsQuery = data.Tickets
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Show.Time)
                .AsQueryable();

            var totalTickets = ticketsQuery.Count();

            ticketsQuery = ticketsQuery 
                .Skip((currentPage - 1) * ticketsPerPage)
                .Take(ticketsPerPage);

            var tickets = ticketsQuery
                .Select(t => new TicketServiceModel
                {
                    ShowId = t.Show.Id,
                    TicketId = t.Id,
                    PlayName = t.Show.Play.Name,
                    StageName = t.Show.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.Show.Time),
                    Status = t.ReservationStatus.Name,
                    TotalPrice = t.Reservations.Select(r => r.Price).Sum(),
                    SeatNumbers = t.Reservations.Select(r => r.Seat).Select(s => s.Number).ToList(),
                    CreatedOn = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.CreatedOn)
                })
                .ToList();

            var model = new TicketQueryModel
            {
                Tickets = tickets,
                CurrentPage = currentPage,
                TotalTickets = totalTickets,
                ShowAll = showAll
            };

            return model;
        }

        public bool Authorize(string userId, string ticketId)
        {
            var user = data.Tickets.Where(t => t.Id == ticketId).Select(t => t.UserId).FirstOrDefault();

            if(userId != user)
            {
                return false;
            }

            return true;
        }

        public bool Confirm(string ticketId, int action)
        {
            var ticket = data.Tickets
                .Where(t => t.Id == ticketId)
                .Select(t => t)
                .FirstOrDefault();

            var isConfirmed = false;

            if (action == 1)
            {
                var reservations = data.Reservations.Where(r => r.TicketId == ticketId).Select(r => r).ToList();

                data.Reservations.RemoveRange(reservations);

                data.Tickets.Remove(ticket);
            }
            if (action == 2)
            {               
                ticket.ReservationStatusId = data.ReservationStatuses.Where(r => r.Name == "Paid").Select(r => r.Id).FirstOrDefault();

                data.Tickets.Update(ticket);

                isConfirmed = true;
            }

            data.SaveChanges();

            return isConfirmed;
 
        }

        public string Create(int showId, IEnumerable<Reservation> reservations, string userId)
        {
            var ticket = new Ticket
            {
                ShowId = showId,
                ReservationStatusId = data.ReservationStatuses.Where(s => s.Name == "Unconfirmed").Select(s => s.Id).FirstOrDefault(),
                UserId = userId
            };

            ticket.Reservations = reservations;

            data.Tickets.Add(ticket);
            data.SaveChanges();

            return ticket.Id;
        }

        public IEnumerable<TicketViewModel> ToPrint(string ticketId)
        {
            var play = data.Tickets.Where(t => t.Id == ticketId).Select(t => t.Show.Play.Name).FirstOrDefault();

            var stage = data.Tickets.Where(t => t.Id == ticketId).Select(t => t.Show.Stage.Name).FirstOrDefault();

            var time = data.Tickets.Where(t => t.Id == ticketId).Select(t => t.Show.Time).FirstOrDefault();

            var timeStr = string.Format(CultureInfo.InvariantCulture, "{0:f}", time);

            var tickets = data.Reservations
                .Where(r => r.TicketId == ticketId)
                .Select(r => new TicketViewModel
                {
                    TicketId = ticketId,
                    PlayName = play,
                    StageName = stage,
                    Time = timeStr,
                    SeatNumber = r.Seat.Number,
                    Row = r.Seat.Row,
                    Price = r.Price
                })
                .OrderBy(r => r.SeatNumber)
                .ToList();

            return tickets;
        }

        public IEnumerable<Reservation> ReserveSeats(int showId, string selectedSeats)
        {
            var reservations = new List<Reservation>();

            var selectedSeatsList = selectedSeats.Split().Select(int.Parse).ToList();

            foreach (var seatId in selectedSeatsList)
            {
                var seatIsTaken = data.Reservations.Where(r => r.ShowId == showId && r.SeatId == seatId).Any();

                if (seatIsTaken)
                {
                    return null;
                }

                reservations.Add(new Reservation
                {
                    SeatId = seatId,
                    ShowId = showId,
                    Price = data.Seats.Select(s => s.SeatCategory).Select(c => c.Price).FirstOrDefault()
                });
            }

            return reservations;
        }

        public TicketServiceModel Review(string ticketId)
        {
            var ticket = data.Tickets
                .Where(t => t.Id == ticketId)
                .Select(t => new TicketServiceModel
                {
                    ShowId = t.Show.Id,
                    TicketId = t.Id,
                    PlayName = t.Show.Play.Name,
                    StageName = t.Show.Stage.Name,
                    Time = string.Format(CultureInfo.InvariantCulture, "{0:f}", t.Show.Time),
                    SeatNumbers = t.Reservations.Select(r => r.Seat).Select(s => s.Number).ToList(),
                    TotalPrice = (decimal)t.Reservations.Select(r => r.Price).Sum()
                })
                .FirstOrDefault();

            return ticket;
        }
    }
}


