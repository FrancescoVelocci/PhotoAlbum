using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using PhotoAlbum.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhotoAlbum.ViewModels
{
    public class SearchViewModel
    {
        public List<ViewPictureHelper> ViewPictureHelpers { get; set; }

        public int LocationID { get; set; }
        public List<SelectListItem> Locations { get; set; }

        public int EventID { get; set; }
        public List<SelectListItem> Events { get; set; }

        public int AuthorID { get; set; }
        public List<SelectListItem> Authors { get; set; }

        public List<People> PeopleList { get; set; }
        
        public SearchViewModel()
        { }

        public SearchViewModel(List<ViewPictureHelper> viewPictureHelpers,
                                IEnumerable<Location> locations,
                                IEnumerable<Event> events,
                                IEnumerable<Author> authors,
                                List<People> peopleList)
        {
            ViewPictureHelpers = viewPictureHelpers;

            Locations = new List<SelectListItem>();
            Locations.Add(new SelectListItem
            {
                Value = "0",
                Text = "   "
            });

            Locations.Add(new SelectListItem
            {
                Value = "null",
                Text = "No Location"
            });

            foreach (var i in locations)
            {
                Locations.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.Nation + ", " + i.City + " - " + i.Place.Name + ": " + i.PlaceName
                });
            }
            

            Events = new List<SelectListItem>();
            Events.Add(new SelectListItem
            {
                Value = "0",
                Text = "   "
            });

            Events.Add(new SelectListItem
            {
                Value = "null",
                Text = "No Event"
            });

            foreach (var i in events)
            {
                Events.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.EventType.Name + ": " + i.Name
                });
            }

            Authors = new List<SelectListItem>();
            Authors.Add(new SelectListItem
            {
                Value = "0",
                Text = "   "
            });

            Authors.Add(new SelectListItem
            {
                Value = "null",
                Text = "Not Author"
            });

            foreach (var i in authors)
            {
                Authors.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.Name + " " + i.LastName
                });
            }

            PeopleList = peopleList;
        }
    }
}
