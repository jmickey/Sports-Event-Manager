using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsManager.ViewModels
{
    public class EventCompetitorViewModel
    {
        public EventCompetitor EventCompetitor { get; set; }

        [Display(Name = "Competitor")]
        public int SelectedCompetitor { get; set; }

        public SelectList AllCompetitors { get; set; }
    }
}