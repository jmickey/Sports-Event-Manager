using SportsManager.Custom;
using SportsManager.Models;
using SportsManager.ViewModels;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "EventManager")]
    public class GameResultsController : Controller
    {
        private SportsContext db = new SportsContext();

        // GET: Medals
        public ActionResult Index()
        {
            // http://stackoverflow.com/a/13846627

            GameResultViewModel viewModel = new GameResultViewModel();

            viewModel.GameResultMedals = db.EventCompetitors
                .Include(c => c.Competitor)
                .GroupBy(c => c.Competitor.competitorCountry)
                .Select(r => new GameResultMedals { Country = r.Key, Gold = r.Count(a => a.competitorMedal == "Gold"),
                    Silver = r.Count(a => a.competitorMedal == "Silver"),
                    Bronze = r.Count(a => a.competitorMedal == "Bronze"),
                    Total = r.Count(a => a.competitorMedal != "None")})
                .OrderByDescending(a => a.Total)
                .ThenBy(a => a.Country).ToList();

            viewModel.GameResultRecords = db.EventCompetitors
                .Include(c => c.Competitor)
                .Include(c => c.Event)
                .Where(ec => ec.Event.worldRecord == true && ec.competitorMedal == "Gold")
                .Select(r => new GameResultRecords { CompetitorName = r.Competitor.competitorName,
                    Game = r.Event.Game.gameName })
                .OrderBy(a => a.CompetitorName).ToList();

            return View(viewModel);
        }
    }
}