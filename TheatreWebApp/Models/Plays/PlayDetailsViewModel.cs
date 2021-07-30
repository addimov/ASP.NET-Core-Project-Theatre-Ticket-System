using System.Collections.Generic;

namespace TheatreWebApp.Models.Plays
{
    public class PlayDetailsViewModel
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
