using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SportsManager.Models;

namespace SportsManager.ViewModels
{
    public class CompetitorViewModel
    {
        public Competitor Competitor { get; set; }

        [Required]
        [Display(Name = "Games Participating")]
        public List<int> SelectedGames { get; set; }

        public MultiSelectList AllGames { get; set; }
    }
}