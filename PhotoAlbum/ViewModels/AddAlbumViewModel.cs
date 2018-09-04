using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.ViewModels
{
    public class AddAlbumViewModel
    {
        [Required]
        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string Date { get; set; }
    }
}
