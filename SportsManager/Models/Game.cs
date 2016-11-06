using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsManager.Models
{
    public class Game
    {
        [Key]
        public int gameID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(30, ErrorMessage = "Game name is too long!")]
        public string gameName { get; set; }

        [Required]
        [Display(Name = "Code")]
        [RegularExpression("^[a-zA-Z]{4}[\\d]{3}$", ErrorMessage = "Invalid game code!")] // 4 upper + 3 numbers
        // Remote validation that checks for uniqueness
        [Remote("IsGameCodeUnique", "Games", AdditionalFields = "gameID", ErrorMessage = "Game code already exists!")]
        public string gameCode { get; set; }

        [Display(Name = "Duration (Mins)")]
        /* A google search shows that the longest olympic event was 12 hours (720 minutes)
        so I decided to use this as the max game duration */
        [Range(0, 720, ErrorMessage = "Must be between 0 and 720 minutes!")]
        public int gameDurationInMins { get; set; }

        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Game description is too long!")]
        public string gameDescription { get; set; }

        [Display(Name = "Rules Booklet")]
        public string gameBookletPath { get; set; }

        // Virtual properties indicate the relationship to the code-first entity framework
        // ICollections denotes that this model has "many" of the specified class - eg. Many events
        public virtual ICollection<Competitor> Competitors { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}