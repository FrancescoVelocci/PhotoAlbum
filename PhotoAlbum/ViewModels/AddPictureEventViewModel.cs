using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoAlbum.Helpers;
using PhotoAlbum.Models;

namespace PhotoAlbum.ViewModels
{
    public class AddPictureEventViewModel
    {
        public List<ViewPictureHelper> ViewPictureHelper { get; set; }
        public int EventID { get; set; }
        public List<SelectListItem> Events { get; set; }

        public List<int> PictureIDs { get; set; }
        public string StackID { get; set; }

        public AddPictureEventViewModel()
        {

        }

        public AddPictureEventViewModel(
                                            List<ViewPictureHelper> viewPictureHelpers,
                                            IEnumerable<Event> events,
                                            List<int> pictureIDs,
                                            string stackID
                                           )

        {
            ViewPictureHelper = viewPictureHelpers;

            Events = new List<SelectListItem>();

            foreach (var i in events)
            {
                Events.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.EventType.Name + ": " + i.Name
                });
            }

            PictureIDs = pictureIDs;

            StackID = stackID;
        }
    }
}

