using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Program
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

        public IEnumerable<ShowQueryPlayModel> Plays { get; set; }

        public IEnumerable<ShowViewModel> Shows { get; set; }
    }
}
