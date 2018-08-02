using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class Stack
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Classified { get; set; }

        public List<Picture> Pictures {get; set;}
    }
}
