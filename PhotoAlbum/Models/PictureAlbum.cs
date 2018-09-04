using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class PictureAlbum
    {
        public int PictureID { get; set; }
        public Picture picture { get; set; }

        public int AlbumID { get; set; }
        public Album Album { get; set; }
    }
}
