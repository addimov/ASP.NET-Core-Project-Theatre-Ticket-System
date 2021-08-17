using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TheatreWebApp.Data;

namespace TheatreWebApp.Services.Background
{
    public class TicketCleanupService : ITicketCleanupService
    {
        private Timer timer;

        public TicketCleanupService(IServiceProvider services)
        {
            Services = services;
        }
        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(20));

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var data = scope.ServiceProvider.GetRequiredService<TheatreDbContext>();

                var unconfirmedTickets = data.Tickets
                   .Where(t => t.ReservationStatus.Name == "Unconfirmed")
                   .ToList();

                if (unconfirmedTickets.Count() == 0)
                {
                    Console.WriteLine("No unconfirmed tickets found.");
                    return;
                }

                var unconfirmedTicketsIds = new List<string>();

                foreach (var ticket in unconfirmedTickets)
                {
                    unconfirmedTicketsIds.Add(ticket.Id);
                }

                var reservations = data.Reservations
                    .Where(r => unconfirmedTicketsIds.Contains(r.TicketId))
                    .ToList();

                data.RemoveRange(reservations);
                data.RemoveRange(unconfirmedTickets);
                data.SaveChanges();

                Console.WriteLine($"{unconfirmedTickets.Count()} tickets removed, freeing up {reservations.Count()} seats.");
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}