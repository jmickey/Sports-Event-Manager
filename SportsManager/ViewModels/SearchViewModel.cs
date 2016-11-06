using SportsManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsManager.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Tag")]
        public int selectedTag { get; set; }
        public ICollection<Photo> SearchResults { get; set; }
        public SelectList AllTags { get; set; }
    }
}