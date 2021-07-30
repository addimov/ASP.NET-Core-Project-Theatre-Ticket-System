using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheatreWebApp.Models.Program
{
    public class EditShowFormModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{2}/[0-9]{2}/[0-9]{4}")]
        public string Date { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{2}:[0-9]{2}")]
        public string Time { get; set; }
    }
}
