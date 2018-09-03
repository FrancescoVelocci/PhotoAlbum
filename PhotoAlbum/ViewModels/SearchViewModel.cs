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
            foreach (var i in locations)
            {
                Locations.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.Nation + ", " + i.City + " - " + i.Place.Name + ": " + i.PlaceName
                });
            }

            Events = new List<SelectListItem>();
            foreach (var i in events)
            {
                Events.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.EventType.Name + ": " + i.Name
                });
            }

            Authors = new List<SelectListItem>();
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
