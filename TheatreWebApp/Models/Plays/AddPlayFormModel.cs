using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Models.Plays
{
    public class AddPlayFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(250)]
        public string ShortDescription { get; set; }
    }
}
