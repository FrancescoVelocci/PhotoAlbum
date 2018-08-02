using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.ViewModels;
using PhotoAlbum.Models;

namespace PhotoAlbum.ViewModels
{
    public class AddEventViewModel
    {
        [Required]
        [Display(Name ="Event Name")]
        public string Name { get; set; }

        [Display(Name = "Event Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Event Type")]
        public int EventTypeID { get; set; }

        public List<SelectListItem> EventTypes { get; set; }

        public AddEventViewModel()
        { }

        public AddEventViewModel(IEnumerable<EventType> eventType)
        {
            EventTypes = new List<SelectListItem>();

            foreach (var i in eventType)
            {
                EventTypes.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.Name
                });
            }
        }
    }
}
