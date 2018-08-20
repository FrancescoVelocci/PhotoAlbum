using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class PictureAuthor
    {
        public int PictureID { get; set; }
        public Picture Picture { get; set; }

        public int AuthorID { get; set; }
        public Author Author { get; set; }
    }
}
