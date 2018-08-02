using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;

namespace PhotoAlbum.ViewModels
{
    public class AddAuthorViewModel
    {
        [Required]
        [Display(Name = "Author's Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Author's LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Professional")]
        public bool Professional { get; set; }
    }
}
