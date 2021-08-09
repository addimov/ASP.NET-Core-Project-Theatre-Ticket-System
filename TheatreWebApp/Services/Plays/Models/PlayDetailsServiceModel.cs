using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Plays.Models
{
    public class PlayDetailsServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Credits { get; set; }

        public string ImageUrl { get; set; }

        public bool IsHidden { get; set; }

        public IEnumerable<string> Paragraphs { get; set; }
    }
}
