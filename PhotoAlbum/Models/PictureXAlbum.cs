using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class PictureXAlbum
    {
        public int AlbumID { get; set; }
        public Album Album { get; set; }

        public int PictureID { get; set; }
        public Picture Picture { get; set; }
    }
}
