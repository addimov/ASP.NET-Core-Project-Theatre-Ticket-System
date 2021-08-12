using System.Collections.Generic;
using TheatreWebApp.Services.Plays.Models;

namespace TheatreWebApp.Models.Plays
{
    public class PlayQueryModel
    {
        public const int PlaysPerPage = 6;

        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPlays { get; set; }

        public IEnumerable<PlayServiceModel> Plays { get; set; }
    }
}
