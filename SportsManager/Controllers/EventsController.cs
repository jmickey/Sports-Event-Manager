using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using SportsManager.Models;
using SportsManager.Custom;
using System.Collections.Generic;
using SportsManager.ViewModels;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace SportsManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "EventManager")]
    public class EventsController : Controller
    {
        private SportsContext db = new SportsContext();

        // GET: Events
        public ActionResult Index(int? id)
        {
            EventIndexViewModel indexViewModel = new EventIndexViewModel();

            indexViewModel.Events = db.Events
                .Include(e => e.Game)
                .Include(e => e.EventCompetitors);

            if (id != null)
            {
                // If the ID is not null (event is selected) then include the information
                // for event competitors
                indexViewModel.EventCompetitors = indexViewModel.Events
                    .Where(e => e.eventID == id).Single().EventCompetitors;
            }

            return View(indexViewModel);
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Include(e => e.Photos).FirstOrDefault(e => e.eventID == id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            EventViewModel eventViewModel = new EventViewModel();
            // Get all games to populate DropDownListFor
            eventViewModel.Games = new SelectList(db.Games, "gameID", "gameName");
            // Source: https://goo.gl/r5MCvb (c-sharpcorner.com)
            return View(eventViewModel);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel eventViewModel, IEnumerable<HttpPostedFileBase> eventPhotos)
        {
            if (eventPhotos.Count() > 0)
            {
                // An array of allowed extensions
                var acceptedExtentions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                foreach (var file in eventPhotos)
                {
                    if (file != null)
                    {
                        // Get extention of the file
                        var fileExtension = Path.GetExtension(file.FileName).ToLower();
                        if (!acceptedExtentions.Contains(fileExtension))
                        {
                            ModelState.AddModelError(string.Empty, $"File ({file.FileName}) is not a valid type!");
                        }
                        else if (file.ContentLength < 1)
                        {
                            ModelState.AddModelError("", $"Error uploading file ({file.FileName}), file is empty or corrupt!");
                        }
                    } else
                    {
                        ModelState.AddModelError("", "You added an additional photo but did not select a file!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "The event requires at least one photo!");
            }

            if (ModelState.IsValid)
            {
                Event @event = eventViewModel.Event;

                int tempNum = 0;
                foreach (var photo in eventPhotos) {
                    var fileExtension = Path.GetExtension(photo.FileName).ToLower();
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    try
                    {
                        // Save the photo
                        photo.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Upload/Photos"), fileName));
                    }
                    catch
                    {
                        // Show an error if file couldn't be saved for whatever reason
                        // In a more traditional application the exception would be logged, however I am not doing that here
                        ModelState.AddModelError("", "File could not be saved, please contact the administrator!");
                    }

                    fileName = "/Upload/Photos/" + fileName;
                    Photo eventPhoto = new Photo()
                    {
                        photoPath = fileName
                    };
                    var splitPattern = "[.,;]\\s*";
                    var tags = Regex.Split(eventViewModel.PhotoTags[tempNum], splitPattern);
                    tags = tags.Distinct().ToArray();
                    foreach (string tag in tags)
                    {
                        // Check if the tag already exists, otherwsie create a new tag
                        var existingTag = db.Tags.FirstOrDefault(t => t.tagString.ToLower() == tag);
                        if (existingTag == null)
                        {
                            Tag newTag = new Tag()
                            {
                                tagString = tag
                            };
                            // Add to the photo
                            eventPhoto.Tags.Add(newTag);
                        } else
                        {
                            // Add to the photo
                            eventPhoto.Tags.Add(existingTag);
                        }
                    }
                    tempNum++;
                    // Add the photo to the event
                    @event.Photos.Add(eventPhoto);

                    /* Note: There is a "bug" here. If the same tag is used more than once, but on a different
                    photo it will create a dusplicate of that tag. Time constraints meant that I didn't have time
                    to fix this */
                }

                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            /* If the model is not valid then we return to the page where the appropriate validation errors are display, 
            if this happens we have to repopulate the Games list in the ViewModel */
            eventViewModel.Games = new SelectList(db.Games, "gameID", "gameName");

            return View(eventViewModel);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Include(e => e.Photos).FirstOrDefault(e => e.eventID == id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);

            if (@event.Photos != null)
            {
                foreach (var photo in @event.Photos)
                {
                    // Delete the file if it exists
                    System.IO.File.Delete(HttpContext.Server.MapPath("~" + photo.photoPath));
                }
            }

            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult SearchTags(string term)
        {
            var result = db.Tags.Where(t => t.tagString.ToLower().StartsWith(term.ToLower())).Select(t => new { t.tagString });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGameDuration(string gameName)
        {
            var result = db.Games.Where(g => g.gameName.ToLower().Equals(gameName.ToLower())).Select(g => g.gameDurationInMins);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
