using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheatreWebApp.Data.Models;

namespace TheatreWebApp.Models.Shows
{
    public class AddShowFormModel
    {
        public int PlayId { get; set; }

        public int StageId { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{2}/[0-9]{2}/[0-9]{4}")]
        public string Date { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{2}:[0-9]{2}")]
        public string Time { get; set; }

        public IEnumerable<Play> Plays { get; set; }

        public IEnumerable<Stage> Stages { get; set; }
    }
}
