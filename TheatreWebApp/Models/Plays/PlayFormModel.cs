using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Models.Plays
{
    public class PlayFormModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "General information")]
        public string ShortDescription { get; set; }

        [MaxLength(250)]
        public string Credits { get; set; }

        [Url]
        public string ImageUrl { get; set; }
    }
}
