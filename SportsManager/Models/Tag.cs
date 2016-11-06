using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsManager.Models
{
    public class Tag
    {
        [Key]
        public int tagID { get; set; }

        public string tagString { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}