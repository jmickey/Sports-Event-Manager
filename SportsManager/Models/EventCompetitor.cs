using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportsManager.Models
{
    /* Because of the additional attributes required the table must be manually modelled
    instead of relying on Entity Framework to work it out by itself. 
    Source: http://stackoverflow.com/a/7053393 */
    public class EventCompetitor
    {
        // Hidden field that is auto populated, so no validation required
        public int eventID { get; set; }

        [Required]
        [Display(Name = "Competitor")]
        public int competitorID { get; set; }

        [Required]
        [Display(Name = "Position")]
        public int competitorPosition { get; set; }

        [Required]
        [Display(Name = "Medal")]
        public string competitorMedal { get; set; }

        public virtual Event Event { get; set; }
        public virtual Competitor Competitor { get; set; }
    }
}