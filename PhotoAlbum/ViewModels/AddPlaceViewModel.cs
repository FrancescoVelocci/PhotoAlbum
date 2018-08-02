using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.ViewModels
{
    public class AddPlaceViewModel
    {
        [Required]
        [Display(Name = "Place")]
        public string Name { get; set; }
    }
}
