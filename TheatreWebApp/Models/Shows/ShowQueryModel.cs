using System;
using System.Collections.Generic;
using System.Linq;
using TheatreWebApp.Services.Shows.Models;

namespace TheatreWebApp.Models.Shows
{
    public class ShowQueryModel
    {
        public const int ShowsPerPage = 10;

        public int PlayId { get; set; }

        public string SearchTerm { get; set; }

        public string AfterDate { get; set; }

        public string BeforeDate { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalShows { get; set; }

        public IEnumerable<ShowServicePlayModel> Plays { get; set; }

        public IEnumerable<ShowServiceModel> Shows { get; set; }
    }
}
