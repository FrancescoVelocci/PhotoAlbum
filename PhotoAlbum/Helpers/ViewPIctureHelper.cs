using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Helpers
{
    public class ViewPictureHelper
    {
        //Picture Model
        public int PictureID { get; set; }
        public string PictureName { get; set; }
        public DateTime PictureDate { get; set; }
        public bool PictureFavorite { get; set; }
        public int PictureStackID { get; set; }
        public bool PictureStackIsClassified { get; set; }

        //Author Model
        public string PictureAuthorID { get; set; }
        public string PictureAuthorName { get; set; }
        public string PictureAuthorLastName { get; set; }

        //Location Model
        public string PictureLocationID { get; set; }
        public string PictureNation { get; set; }
        public string PictureCity { get; set; }
        public string PicturePlaceType { get; set; }
        public string PicturePlaceName { get; set; }

        //Event Model
        public string PictureEventID { get; set; }
        public string PictureEventName { get; set; }
        public string PictureEventType { get; set; }

        //People Model
        public string PicturePeopleIDs { get; set; }
    }   
}
