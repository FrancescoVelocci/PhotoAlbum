using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class Place
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<Location> Locations { get; set; }
    }
}
