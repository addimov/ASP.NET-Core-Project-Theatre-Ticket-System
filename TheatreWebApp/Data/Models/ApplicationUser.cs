using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TheatreWebApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
