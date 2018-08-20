using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string Nation { get; set; }
        public string City { get; set; }
        public string PlaceName { get; set; }

        public int PlaceID { get; set; }
        public Place Place { get; set; }

        public IList<PictureLocation> PictureLocations { get; set; }
    }
}
