using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class PicturePeople
    {
        public int PictureID { get; set; }
        public Picture Picture { get; set; }

        public int PeopleID { get; set; }
        public People People { get; set; }
    }
}
