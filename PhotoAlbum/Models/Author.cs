using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Professional { get; set; }
    }
}
