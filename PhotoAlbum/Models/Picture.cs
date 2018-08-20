using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Models
{
    public class Picture
    {
        public int ID { get; set; }
        public string StackName { get; set; }

        [Required]
        public string Name { get; set; }
        public bool Favorite { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public int StackID { get; set; }
        public Stack Stack { get; set; }

        public IList<PictureAuthor> PictureAuthors { get; set; }

        public IList<PictureLocation> PictureLocations { get; set; }

        public IList<PictureEvent> PictureEvent { get; set; }

        public IList<PicturePeople> PicturePeople { get; set; }
    }
}
