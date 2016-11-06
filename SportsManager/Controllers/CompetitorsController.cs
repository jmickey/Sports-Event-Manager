using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SportsManager.Models;
using SportsManager.ViewModels;
using SportsManager.Custom;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace SportsManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class CompetitorsController : Controller
    {
        private SportsContext db = new SportsContext();

        /* This following resources was very helpful when implementing this controller:
        - http://www.codeproject.com/Articles/1063846/Step-By-Step-Implementation-of-MultiSelectList-In
        - http://stackoverflow.com/a/11593957 
        - http://www.codeproject.com/Articles/702890/MVC-Entity-Framework-and-Many-to-Many-Relation */

        // GET: Competitors
        public ActionResult Index()
        {
            // Fetch the data from the DB, including the games the competitor is participating in
            var competitors = db.Competitors.Include(c => c.Games);
            return View(competitors);
        }

        // GET: Competitors/Details/id
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the relevant record within the database based on the ID that is received
            Competitor competitor = db.Competitors
                .Include(c => c.Games)
                .FirstOrDefault(c => c.competitorID == id);

            if (competitor == null)
            {
                return HttpNotFound();
            }

            return View(competitor);
        }

        // GET: Competitors/Create
        public ActionResult Create()
        {
            // MultiSelectList will hold all the available games a competitor can participate in
            MultiSelectList gamesList = new MultiSelectList(db.Games, "gameID", "gameName");
            CompetitorViewModel competitorViewModel = new CompetitorViewModel { AllGames = gamesList };

            ViewBag.countryList = GetCountries();
            ViewBag.titlesList = GetTitles();
            ViewBag.genderList = GetGenders();
            return View(competitorViewModel);
        }

        // POST: Competitors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetitorViewModel competitorViewModel, HttpPostedFileBase competitorPhoto)
        {
            if (competitorPhoto != null)
            {
                // An array of allowed extensions
                var acceptedExtentions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                // Get extention of the file
                var fileExtension = Path.GetExtension(competitorPhoto.FileName).ToLower();
                if (!acceptedExtentions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "File is not a valid type!");
                }
                // Check if the file has any content. A 0 byte file will return invalid to the view
                if (competitorPhoto.ContentLength < 1)
                {
                    ModelState.AddModelError("", "Error uploading file, file is empty or corrupt!");
                }
            }

            if (ModelState.IsValid)
            {
                // Populate competitor object with form data
                Competitor competitor = competitorViewModel.Competitor;
                // Convert email to lowercase
                competitor.competitorEmail = competitor.competitorEmail.ToLower();

                if (competitorViewModel.SelectedGames != null)
                {
                    foreach (var gameID in competitorViewModel.SelectedGames)
                    {
                        // Find the game by ID
                        Game game = db.Games.Find(gameID);
                        competitor.Games.Add(game);
                    }
                }

                if (competitorPhoto != null)
                {
                    var fileExtension = Path.GetExtension(competitorPhoto.FileName).ToLower();
                    string fileNamePattern = @"\s|[().,]";
                    var fileName = Regex.Replace(competitor.competitorName,
                            fileNamePattern, string.Empty)
                        + fileExtension;
                    try
                    {
                        competitorPhoto.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Upload/Photos"), fileName));
                        competitor.competitorPhotoPath = "/Upload/Photos/" + fileName;
                    } catch
                    {
                        // Show an error if file couldn't be saved for whatever reason
                        // In a more traditional application the exception would be logged, however I am not doing that here
                        ModelState.AddModelError("", "File could not be saved, please contact the administrator!");
                    }
                }

                // Verify the ModelState remains valid (try-catch block above could change it)
                if (ModelState.IsValid)
                {
                    // Save changes to the DB
                    db.Competitors.Add(competitor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            // Repopulate the ViewBag/AllGames variables if the form is not valid and has to be re-displayed
            MultiSelectList gamesList = new MultiSelectList(db.Games, "gameID", "gameName");
            competitorViewModel.AllGames = gamesList;
            ViewBag.countryList = GetCountries();
            ViewBag.titlesList = GetTitles();
            ViewBag.genderList = GetGenders();

            return View(competitorViewModel);
        }

        // GET: Competitors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CompetitorViewModel competitorViewModel = new CompetitorViewModel();
            competitorViewModel.Competitor = db.Competitors
                .Include(c => c.Games)
                .FirstOrDefault(c => c.competitorID == id);

            if (competitorViewModel.Competitor == null)
            {
                return HttpNotFound();
            }

            var allGames = db.Games.ToList();
            
            competitorViewModel.Competitor.Games = allGames
                .Where(g => g.Competitors
                .Contains(competitorViewModel.Competitor))
                .ToList();
            List<int> selectedGames = new List<int>();

            foreach (Game game in competitorViewModel.Competitor.Games)
            {
                selectedGames.Add(game.gameID);
            }

            // Populate the variables required for dropdown lists
            MultiSelectList gamesList = new MultiSelectList(allGames, "gameID", "gameName", selectedGames);
            competitorViewModel.AllGames = gamesList;
            ViewBag.countryList = GetCountries();
            ViewBag.titlesList = GetTitles();
            ViewBag.genderList = GetGenders();

            return View(competitorViewModel);
        }

        // POST: Competitors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetitorViewModel model, HttpPostedFileBase competitorPhoto)
        {
            if (competitorPhoto != null)
            {
                // An array of allowed extensions
                var acceptedExtentions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                var fileExtension = Path.GetExtension(competitorPhoto.FileName).ToLower();
                if (!acceptedExtentions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "File is not a valid type!");
                }
                // Check if the file has any content. A 0 byte file will return invalid to the view
                if (!(competitorPhoto.ContentLength > 0))
                {
                    ModelState.AddModelError("", "Error uploading file, file is empty or corrupt!");
                }
            }

            if (ModelState.IsValid)
            {
                // Source: http://stackoverflow.com/a/17819169
                // Retrieve the current data for this competitor, including current games
                Competitor competitor = db.Competitors
                    .Include(c => c.Games)
                    .SingleOrDefault(c => c.competitorID == model.Competitor.competitorID);

                // Populate the competitor object within the DB context with the POST data
                db.Entry(competitor).CurrentValues.SetValues(model.Competitor);
                // Convert email to lowercase for consistency
                competitor.competitorEmail = competitor.competitorEmail.ToLower();
                /* Compare the existing games for this competitor with the selected games
                and remove all games that are not in the SelectedGames list */
                competitor.Games.Where(g => !model.SelectedGames.Contains(g.gameID))
                    .ToList()
                    .ForEach(game => competitor.Games.Remove(game));

                foreach (int gameID in model.SelectedGames)
                {
                    if (!competitor.Games.Any(g => g.gameID == gameID))
                    {
                        Game addedGame = new Game { gameID = gameID };
                        // Attach the game to the current DbContext
                        db.Games.Attach(addedGame);
                        // Add the game to the competitor object
                        competitor.Games.Add(addedGame);
                    }
                }

                if (competitorPhoto != null)
                {
                    var fileExtention = Path.GetExtension(competitorPhoto.FileName).ToLower();
                    string fileNamePattern = @"\s|[().,]";
                    var fileName = Regex.Replace(competitor.competitorName,
                            fileNamePattern, string.Empty)
                        + fileExtention;

                    try
                    {
                        if (competitor.competitorPhotoPath != null)
                        {
                            System.IO.File.Delete(HttpContext.Server.MapPath("~" + competitor.competitorPhotoPath));
                        }
                        competitorPhoto.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Upload/Photos"), fileName));
                        competitor.competitorPhotoPath = "/Upload/Photos/" + fileName;
                    }
                    catch
                    {
                        ModelState.AddModelError("", "File could not be saved, please contact the administrator!");
                    }
                }

                // Verify the ModelState remains valid (try-catch block above could change it)
                if (ModelState.IsValid)
                {
                    db.Entry(competitor).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            model.AllGames = new MultiSelectList(db.Games, "gameID", "gameName", model.SelectedGames);
            ViewBag.countryList = GetCountries();
            ViewBag.titlesList = GetTitles();
            ViewBag.genderList = GetGenders();

            return View(model);
        }

        private void UpdateGames(int id, List<int> SelectedGames)
        {
            // Source: http://stackoverflow.com/a/12446424
            var originalCompetitor = db.Competitors.Include("Games").Single(c => c.competitorID == id);
            originalCompetitor.Games.Clear();
            foreach (int gameID in SelectedGames)
            {
                Game game = db.Games.Local.SingleOrDefault(g => g.gameID == gameID);
                originalCompetitor.Games.Add(game);
            }

            db.SaveChanges();
        }

        // GET: Competitors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Competitor competitor = db.Competitors
                .Include(c => c.Games)
                .FirstOrDefault(c => c.competitorID == id);

            if (competitor == null)
            {
                return HttpNotFound();
            }

            return View(competitor);
        }

        // POST: Competitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Competitor competitor = db.Competitors
                .Include(c => c.Games)
                .FirstOrDefault(c => c.competitorID == id);

            if (competitor.competitorPhotoPath != null)
            {
                // Delete the file if it exists
                System.IO.File.Delete(HttpContext.Server.MapPath("~" + competitor.competitorPhotoPath));
            }

            db.Competitors.Remove(competitor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Source: http://exploitsincode.blogspot.com.au/2012/10/country-list-for-aspnet-mvc.html
        public IEnumerable<SelectListItem> GetCountries()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<SelectListItem> countryNames = new List<SelectListItem>();

            //To get the Country Names from the CultureInfo installed in windows
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(new SelectListItem() { Text = country.DisplayName, Value = country.DisplayName });
            }

            //Assigning all Country names to IEnumerable
            IEnumerable<SelectListItem> countryList = countryNames
                .GroupBy(x => x.Text)
                .Select(x => x.FirstOrDefault())
                .ToList<SelectListItem>()
                .OrderBy(x => x.Text);
            return countryList;
        }

        private ICollection<SelectListItem> GetTitles()
        {
            List<SelectListItem> titles = new List<SelectListItem>();

            titles.Add(new SelectListItem() { Text = "Mr", Value = "Mr" });
            titles.Add(new SelectListItem() { Text = "Mrs", Value = "Mrs" });
            titles.Add(new SelectListItem() { Text = "Miss", Value = "Miss" });
            titles.Add(new SelectListItem() { Text = "Ms", Value = "Ms" });
            titles.Add(new SelectListItem() { Text = "Master", Value = "Master" });
            titles.Add(new SelectListItem() { Text = "Dr", Value = "Dr" });
            titles.Add(new SelectListItem() { Text = "Sir", Value = "Sir" });
            titles.Add(new SelectListItem() { Text = "Madam", Value = "Madam" });

            return titles;
        }

        private ICollection<SelectListItem> GetGenders()
        {
            List<SelectListItem> genders = new List<SelectListItem>();

            genders.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            genders.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            genders.Add(new SelectListItem() { Text = "Attack Helicopter", Value = "Attack Helicopter" });
            
            return genders;
        }

        // http://www.c-sharpcorner.com/uploadfile/219d4d/how-to-check-if-the-username-is-unique-or-not-in-mvc-5/
        public JsonResult IsEmailUnique(
            [Bind(Prefix = "Competitor.competitorEmail")]string competitorEmail,
            [Bind(Prefix = "Competitor.competitorID")]int? competitorID
        )
        {
            var emailItem = db.Competitors
                .FirstOrDefault(g => g.competitorEmail == competitorEmail.ToLower());
            if (emailItem != null)
            {
                return Json(emailItem.competitorID == competitorID, JsonRequestBehavior.AllowGet);
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
