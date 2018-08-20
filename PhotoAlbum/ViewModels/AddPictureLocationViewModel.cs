using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoAlbum.Models;
using System;
using PhotoAlbum.Helpers;

namespace PhotoAlbum.ViewModels
{
    public class AddPictureLocationViewModel
    {

        public List<ViewPictureHelper> ViewPictureHelper { get; set; }
        public int LocationID { get; set; }
        public List<SelectListItem> Locations { get; set; }

        public List<int> PictureIDs { get; set; }
        public string StackID { get; set; }

        public AddPictureLocationViewModel()
        {

        }

        public AddPictureLocationViewModel(
                                            List<ViewPictureHelper> viewPictureHelpers,
                                            IEnumerable<Location> locations,
                                            List<int> pictureIDs,
                                            string stackID
                                           )

        {
            ViewPictureHelper = viewPictureHelpers;

            Locations = new List<SelectListItem>();

            foreach (var i in locations)
            {
                Locations.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.City
                });
            }

            PictureIDs = pictureIDs;

            StackID = stackID;
        }
    }
}


