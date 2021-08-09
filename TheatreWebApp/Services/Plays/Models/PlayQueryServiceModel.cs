using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Services.Plays.Models
{
    public class PlayQueryServiceModel
    {
        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; }

        public int PlaysPerPage { get; set; }

        public int TotalPlays { get; set; }

        public bool ShowHidden { get; set; }

        public IEnumerable<PlayServiceModel> Plays { get; set; }
    }
}
