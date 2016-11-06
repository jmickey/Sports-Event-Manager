using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsManager.Models
{
    public class Competitor
    {
        public Competitor()
        {
            this.Games = new List<Game>();
            this.EventCompetitors = new List<EventCompetitor>();
        }

        [Key]
        public int competitorID { get; set; }

        [Display(Name = "Title")]
        public string competitorSalutation { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string competitorName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime competitorDoB { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        [Remote("IsEmailUnique", "Competitors", AdditionalFields = "competitorID", ErrorMessage = "Email address already in use!")]
        public string competitorEmail { get; set; }

        [Display(Name = "Description")]
        [StringLength(100)]
        public string competitorDescription { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string competitorCountry { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string competitorGender { get; set; }

        [Display(Name = "Phone")]
        // Attempt to validate the phone input to a reasonable degree
        [RegularExpression("^[+]*[0-9 )(-]{3,18}$", ErrorMessage = "Invalid phone number!")]
        public string competitorPhone { get; set; }

        [Url]
        [Display(Name = "Website")]
        public string competitorWebsite { get; set; }

        [Display(Name = "Photo")]
        public string competitorPhotoPath { get; set; }

        // Relationships with other models. Collections indicate there are many of that object per event
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<EventCompetitor> EventCompetitors { get; set; }
    }
}