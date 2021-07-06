using System;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApp.Data.Models
{
    public class Show
    {
        [Key]
        public int Id { get; init; }

        public int PlayId { get; set; }

        public Play Play { get; set; }

        public int StageId { get; set; }

        public Stage Stage { get; set; }

        public DateTime Time { get; set; }
    }
}
