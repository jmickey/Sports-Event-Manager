using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SportsManager.Models;
using SportsManager.Custom;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SportsManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class GamesController : Controller
    {
        private SportsContext db = new SportsContext();

        // GET: Games
        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find game to parse to view based on ID
            Game game = db.Games.Find(id);

            if (game == null)
            {
                return HttpNotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "gameID,gameName,gameCode,gameDurationInMins,gameDescription,gameBookletPath")] Game game, 
            HttpPostedFileBase gameBookletPath
        )
        {
            if (gameBookletPath != null)
            {
                // An array of allowed extensions
                // File upload source: http://rachelappel.com/upload-and-download-files-using-asp-net-mvc/
                var acceptedExtentions = new[] { ".pdf", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(gameBookletPath.FileName).ToLower();
                if (!acceptedExtentions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "File is not a valid type!");
                }
                // Check if the file has any content. A 0 byte file will return invalid to the view
                if (!(gameBookletPath.ContentLength > 0))
                {
                    ModelState.AddModelError("", "Error uploading file, file is empty or corrupt!");
                }
            } else
            {
                ModelState.AddModelError("", "You must include a rules booklet!");
            }

            if (ModelState.IsValid)
            {
                if (gameBookletPath != null)
                {
                    var fileExtension = Path.GetExtension(gameBookletPath.FileName).ToLower();
                    string fileNamePattern = @"\s|[().,]";
                    var fileName = Regex.Replace(game.gameCode,
                            fileNamePattern, string.Empty)
                        + fileExtension;

                    try
                    {
                        gameBookletPath.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Upload/Rules"), fileName));
                        game.gameBookletPath = "/Upload/Rules/" + fileName;
                    }
                    catch
                    {
                        // Show an error if file couldn't be saved for whatever reason
                        // In a more traditional application the exception would be logged, however I am not doing that here
                        ModelState.AddModelError("", "File could not be saved, please contact the administrator!");
                    }
                }

                game.gameCode = game.gameCode.ToUpper();

                // Verify the ModelState remains valid (try-catch block above could change it)
                if (ModelState.IsValid)
                {
                    db.Games.Add(game);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "gameID,gameName,gameCode,gameDurationInMins,gameDescription,gameBookletPath")] Game game, 
            HttpPostedFileBase gameBookletPath
        )
        {
            if (gameBookletPath != null)
            {
                // An array of allowed extensions
                var acceptedExtentions = new[] { ".pdf", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(gameBookletPath.FileName).ToLower();
                if (!acceptedExtentions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "File is not a valid type!");
                }
                // Check if the file has any content. A 0 byte file will return invalid to the view
                if (!(gameBookletPath.ContentLength > 0))
                {
                    ModelState.AddModelError("", "Error uploading file, file is empty or corrupt!");
                }
            } else if (game.gameBookletPath == null)
            {
                /* If, for whatever reason, there is not currently a rules booklet stored for this game
                then reject the edit and advise the user a booklet must be uploaded */
                ModelState.AddModelError("", "You must include a rules booklet!");
            }

            if (ModelState.IsValid)
            {
                if (gameBookletPath != null)
                {
                    var fileExtension = Path.GetExtension(gameBookletPath.FileName).ToLower();
                    string fileNamePattern = @"\s|[().,]";
                    var fileName = Regex.Replace(game.gameCode,
                            fileNamePattern, string.Empty)
                        + fileExtension;

                    try
                    {
                        if (game.gameBookletPath != null)
                        {
                            System.IO.File.Delete(HttpContext.Server.MapPath("~" + game.gameBookletPath));
                        }
                        gameBookletPath.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Upload/Rules"), fileName));
                        game.gameBookletPath = "/Upload/Rules/" + fileName;
                    }
                    catch
                    {
                        // Show an error if file couldn't be saved for whatever reason
                        // In a more traditional application the exception would be logged, however I am not doing that here
                        ModelState.AddModelError("", "File could not be saved, please contact the administrator!");
                    }
                }

                // Verify the ModelState remains valid (try-catch block above could change it)
                if (ModelState.IsValid)
                {
                    db.Entry(game).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);

            // Find any events assigned to that game
            List<Event> events = db.Events.Where(e => e.gameID == game.gameID).ToList();
            // Find any competitors that are participating in the game
            List<Competitor> competitors = db.Competitors.Include(c => c.Games).Where(c => c.Games.Any(g => g.gameID == game.gameID)).ToList();

            // Only allow for the deletion of a game is there are no associated events or competitors
            if (events.Count == 0 && competitors.Count == 0)
            {
                if (game.gameBookletPath != null)
                {
                    // Delete the file if it exists
                    System.IO.File.Delete(HttpContext.Server.MapPath("~" + game.gameBookletPath));
                }

                db.Games.Remove(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                ModelState.AddModelError(string.Empty, "This game is associated with one or more events or competitors and cannot be deleted!" 
                    + " Edit or remove these items and try again.");
                return View("Delete", game);
            }
        }

        // Source: http://www.c-sharpcorner.com/uploadfile/219d4d/how-to-check-if-the-username-is-unique-or-not-in-mvc-5/
        public JsonResult IsGameCodeUnique(string gameCode, int? gameID)
        {
            var gameItem = db.Games.FirstOrDefault(g => g.gameCode == gameCode);

            if (gameItem != null)
            {
                return Json(gameItem.gameID == gameID, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
                
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
