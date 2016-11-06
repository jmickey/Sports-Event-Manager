using SportsManager.Custom;
using SportsManager.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsManager.ViewModels;
using Rotativa;

namespace SportsManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "EventManager")]
    public class SearchController : Controller
    {
        private SportsContext db = new SportsContext();

        // GET: Search
        public ActionResult Index(SearchViewModel viewModel)
        {
            viewModel.AllTags = new SelectList (db.Tags, "tagID", "tagString");

            if (viewModel.selectedTag > 0)
            {
                viewModel.SearchResults = db.Photos.Include(p => p.Tags).Where(p => p.Tags.Any(t => t.tagID == viewModel.selectedTag)).ToList();
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult GetEvents(string searchString)
        {
            // Source https://github.com/webgio/Rotativa
            List<Event> events = db.Events.Where(e => e.eventDescription.Contains(searchString)).ToList();
            return new ViewAsPdf(events)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--viewport-size 980"
            };
        }

    }
}