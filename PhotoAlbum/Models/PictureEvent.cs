using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PhotoAlbum.Models
{
    public class PictureEvent
    {
        public int PictureID { get; set; }
        public Picture Picture { get; set; }

        public int EventID { get; set; }
        public Event Event { get; set; }
    }
}
