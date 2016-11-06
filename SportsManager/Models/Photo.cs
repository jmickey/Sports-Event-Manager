using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsManager.Models
{
    public class Photo
    {
        public Photo()
        {
            Tags = new List<Tag>();
        }
        [Key]
        public int photoID { get; set; }

        public string photoPath { get; set; }

        public int eventID { get; set; }

        public Event Event { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}