using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.ViewModels
{
    public class AddEventTypeViewModel
    {
        [Required]
        [Display(Name = "Event Type Name")]
        public string Name { get; set; }
    }
}
