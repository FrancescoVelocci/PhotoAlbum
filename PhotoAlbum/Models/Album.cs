using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class Album
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        public IList<PictureAlbum> PictureAlbums { get; set; }
    }
}
