using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsManager.Models
{
    public class Event
    {
        public Event()
        {
            Photos = new List<Photo>();
        }

        [Key]
        public int eventID { get; set; }

        [Required]
        [Display(Name = "Game")]
        public int gameID { get; set; }

        [Display(Name = "Feature Event")]
        public bool featureEvent { get; set; }

        [Required]
        [Display(Name = "Venue")]
        [StringLength(100)]
        public string eventVenue { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime eventDate { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime eventStartTime { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime eventEndTime { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Event description is too long!")]
        public string eventDescription { get; set; }

        [Display(Name = "World Record?")]
        public bool worldRecord { get; set; }

        // Relationships with other models. Collections indicate there are many of that object per event
        public virtual Game Game { get; set; }
        public virtual ICollection<EventCompetitor> EventCompetitors { get; set; }
        [Display(Name = "Photo(s)")]
        public virtual ICollection<Photo> Photos { get; set; }
    }
}