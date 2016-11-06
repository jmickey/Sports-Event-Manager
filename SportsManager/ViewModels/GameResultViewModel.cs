using SportsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsManager.ViewModels
{
    public class GameResultMedals
    {
        public string Country { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        public int Total { get; set; }
    }

    public class GameResultRecords
    {
        public string CompetitorName { get; set; }
        public string Game { get; set; }
    }

    public class GameResultViewModel
    {
        public IEnumerable<GameResultMedals> GameResultMedals { get; set; }
        public IEnumerable<GameResultRecords> GameResultRecords { get; set; }
    }
}