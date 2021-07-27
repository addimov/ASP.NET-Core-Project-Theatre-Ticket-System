using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TheatreWebApp.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
