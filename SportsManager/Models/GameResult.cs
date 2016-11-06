using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsManager.Models
{
    public class GameResult
    {
        public string Country { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        public int Total { get; set; }
    }
}