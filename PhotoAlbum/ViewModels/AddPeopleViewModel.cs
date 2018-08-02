using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;

namespace PhotoAlbum.ViewModels
{
    public class AddPeopleViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Relation")]
        public string Relation { get; set; }

        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }
    }
}
