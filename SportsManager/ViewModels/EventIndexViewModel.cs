using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsManager.Models;

namespace SportsManager.ViewModels
{
    public class EventIndexViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<EventCompetitor> EventCompetitors { get; set; }
    }
}