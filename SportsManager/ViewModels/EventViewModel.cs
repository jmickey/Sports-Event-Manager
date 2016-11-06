using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsManager.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportsManager.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public SelectList Games { get; set; }

        [Required]
        [RegularExpression("^[-A-z,.; ]*$", ErrorMessage = "Invalid characters detected in photo tags!")]
        public string[] PhotoTags { get; set; }
    }
}