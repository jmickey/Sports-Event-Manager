using SportsManager.Custom;
using SportsManager.Models;
using SportsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SportsManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "EventManager")]
    public class EventCompetitorsController : Controller
    {
        private SportsContext db = new SportsContext();

        // GET: EventCompetitors/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event eventObject = db.Events.Include(e => e.Game).FirstOrDefault(e => e.eventID == id);

            if (eventObject == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventCompetitor eventCompetitor = new EventCompetitor
            {
                Event = eventObject,
                eventID = eventObject.eventID
            };
            SelectList allCompetitors = new SelectList(eventObject.Game.Competitors, "competitorID", "competitorName");
            EventCompetitorViewModel eventCompetitorViewModel = new EventCompetitorViewModel()
            {
                EventCompetitor = eventCompetitor,
                AllCompetitors = allCompetitors
            };

            ViewBag.Medals = GetMedals();

            return View(eventCompetitorViewModel);
        }

        // POST: EventCompetitors/Create/1
        [HttpPost]
        public ActionResult Create(EventCompetitorViewModel eventCompetitorViewModel)
        {

            if (ModelState.IsValid)
            {
                // Get existing competitor and event from database
                Competitor competitor = db.Competitors
                    .Find(eventCompetitorViewModel.EventCompetitor.competitorID);
                Event @event = db.Events
                    .Find(eventCompetitorViewModel.EventCompetitor.eventID);
                // Assign the secondary propertues
                var competitorPosition = eventCompetitorViewModel.EventCompetitor.competitorPosition;
                var competitorMedal = eventCompetitorViewModel.EventCompetitor.competitorMedal;

                if (competitor != null & @event != null)
                {
                    // Create a new object
                    EventCompetitor eventCompetitor = new EventCompetitor()
                    {
                        Event = @event,
                        Competitor = competitor,
                        competitorMedal = competitorMedal,
                        competitorPosition = competitorPosition
                    };

                    // Add to the DbContext and save the changes
                    db.EventCompetitors.Add(eventCompetitor);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Events", new { id = eventCompetitor.eventID });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid event or competitor!");
                }
            }

            // Repopulate in case of validation error
            Event eventObject = db.Events
                .Include(e => e.Game)
                .FirstOrDefault(e => e.eventID == eventCompetitorViewModel.EventCompetitor.eventID);
            SelectList allCompetitors = new SelectList(eventObject.Game.Competitors,
                "competitorID", "competitorName");
            eventCompetitorViewModel.AllCompetitors = allCompetitors;
            ViewBag.Medals = GetMedals();

            return View(eventCompetitorViewModel);
        }

        // GET: EventCompetitors/Edit/1/2
        public ActionResult Edit(int? id, int? competitorID)
        {
            if (id == null || competitorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the event competitor from the database based on the route parameters
            EventCompetitor eventCompetitor = db.EventCompetitors
                .Include(ec => ec.Competitor)
                .Include(ec => ec.Event)
                .Where(ec => ec.eventID == id && ec.competitorID == competitorID)
                .Single();

            if (eventCompetitor == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventCompetitorViewModel eventCompetitorViewModel = new EventCompetitorViewModel()
            {
                EventCompetitor = eventCompetitor,
            };

            ViewBag.Medals = GetMedals();

            return View(eventCompetitorViewModel);
        }

        // POST: EventCompetitors/Edit/1/2
        [HttpPost]
        public ActionResult Edit(EventCompetitorViewModel eventCompetitorViewModel)
        {
            if (ModelState.IsValid)
            {
                Competitor competitor = db.Competitors
                    .Find(eventCompetitorViewModel.EventCompetitor.competitorID);
                Event @event = db.Events
                    .Find(eventCompetitorViewModel.EventCompetitor.eventID);
                var competitorPosition = eventCompetitorViewModel.EventCompetitor.competitorPosition;
                var competitorMedal = eventCompetitorViewModel.EventCompetitor.competitorMedal;
                if (competitorMedal == "None")
                {
                    competitorMedal = null;
                }

                if (competitor != null & @event != null)
                {
                    EventCompetitor eventCompetitor = new EventCompetitor()
                    {
                        Event = @event,
                        Competitor = competitor,
                        competitorMedal = competitorMedal,
                        competitorPosition = competitorPosition
                    };

                    db.Entry(eventCompetitor).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Events", new { id = eventCompetitor.eventID });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid event or competitor!");
                }
            }

            ViewBag.Medals = GetMedals();

            return View(eventCompetitorViewModel);
        }

        // GET: EventCompetitors/Delete/1/2
        public ActionResult Delete(int? id, int? competitorID)
        {
            if (id == null || competitorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the event competitor from the database based on the route parameters
            EventCompetitor eventCompetitor = db.EventCompetitors
                .Include(ec => ec.Competitor)
                .Include(ec => ec.Event)
                .Where(ec => ec.eventID == id && ec.competitorID == competitorID)
                .Single();

            if (eventCompetitor == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(eventCompetitor);
        }

        // POST: EventCompetitors/Delete/1/2
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, int? competitorID)
        {
            EventCompetitor eventCompetitor = db.EventCompetitors
                .Include(ec => ec.Competitor)
                .Include(ec => ec.Event)
                .Where(ec => ec.eventID == id && ec.competitorID == competitorID)
                .Single();

            db.EventCompetitors.Remove(eventCompetitor);
            db.SaveChanges();
            return RedirectToAction("Index", "Events", new { id = eventCompetitor.eventID });
        }

        private SelectList GetMedals()
        {
            SelectList medals = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem {Text = "Gold", Value ="Gold"},
                    new SelectListItem {Text = "Silver", Value ="Silver"},
                    new SelectListItem {Text = "Bronze", Value ="Bronze"},
                    new SelectListItem {Text = "None", Value ="None"}
                }, "Value", "Text");

            return medals;
        }
    }
}
