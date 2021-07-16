using System.Collections.Generic;

namespace TheatreWebApp.Models.Plays
{
    public class PlayDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public IEnumerable<string> Paragraphs { get; set; }
    }
}
