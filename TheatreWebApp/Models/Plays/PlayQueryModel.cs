using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Plays
{
    public class PlayQueryModel
    {
        public const int PlaysPerPage = 3;

        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPlays { get; set; }

        public IEnumerable<PlaysListViewModel> Plays { get; set; }
    }
}
