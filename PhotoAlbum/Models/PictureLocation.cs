using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class PictureLocation
    {
        public int PictureID { get; set; }
        public Picture Picture { get; set; }

        public int LocationID { get; set; }
        public Location Location { get; set; }
    }
}
