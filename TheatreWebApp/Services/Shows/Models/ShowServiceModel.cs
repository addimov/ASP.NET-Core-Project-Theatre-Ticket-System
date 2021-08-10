using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Shows.Models
{
    public class ShowServiceModel
    {
        public int Id { get; set; }

        public string PlayName { get; set; }

        public int PlayId { get; set; }

        public string StageName { get; set; }

        public string ImageUrl { get; set; }

        public string Time { get; set; }
    }
}
