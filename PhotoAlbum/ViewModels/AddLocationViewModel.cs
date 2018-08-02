using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoAlbum.Models;

namespace PhotoAlbum.ViewModels
{
    public class AddLocationViewModel
    {
        [Required]
        [Display(Name = "Nation")]
        public string Nation { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Place Name")]
        public string PlaceName { get; set; }

        [Required]
        [Display(Name = "Place")]
        public int PlaceID { get; set; }

        public List<SelectListItem> Places { get; set; }

        public AddLocationViewModel()
        { }

        public AddLocationViewModel(IEnumerable<Place> places)
        {
            Places = new List<SelectListItem>();

            foreach (var i in places)
            {
                Places.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.Name
                });
            }
        }
    }
}
