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
    public class AddPicturePeopleViewModel
    {
        public List<ViewPictureHelper> ViewPictureHelper { get; set; }
        public List<string> PicturePeopleNames { get; set; }
        public string StackID { get; set; }
        public List<int> PictureIDs { get; set; }
        public List<People> PeopleList { get; set; }
        public List<int> PeopleIDs { get; set; }

        public AddPicturePeopleViewModel()
        {

        }

        public AddPicturePeopleViewModel(
                                            List<ViewPictureHelper> viewPictureHelpers,
                                            List<string> picturePeopleName,
                                            List<int> pictureIDs,
                                            string stackID,
                                            List<People> peopleList,
                                            List<int> peopleIDs
                                           )

        {
            ViewPictureHelper = viewPictureHelpers;

            PicturePeopleNames = picturePeopleName;

            PictureIDs = pictureIDs;

            StackID = stackID;

            PeopleList = peopleList;

            PeopleIDs = peopleIDs;
        }
    }
}

