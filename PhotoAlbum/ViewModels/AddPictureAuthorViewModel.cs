using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoAlbum.Helpers;
using PhotoAlbum.Models;

namespace PhotoAlbum.ViewModels
{
    public class AddPictureAuthorViewModel
    {
        public List<ViewPictureHelper> ViewPictureHelper { get; set; }
        public int AuthorID { get; set; }
        public List<SelectListItem> Authors { get; set; }

        public List<int> PictureIDs { get; set; }
        public string StackID { get; set; }

        public AddPictureAuthorViewModel()
        {

        }

        public AddPictureAuthorViewModel(
                                            List<ViewPictureHelper> viewPictureHelpers,
                                            IEnumerable<Author> authors,
                                            List<int> pictureIDs,
                                            string stackID
                                           )

        {
            ViewPictureHelper = viewPictureHelpers;

            Authors = new List<SelectListItem>();

            foreach (var i in authors)
            {
                Authors.Add(new SelectListItem
                {
                    Value = i.ID.ToString(),
                    Text = i.Name + " " + i.LastName
                });
            }

            PictureIDs = pictureIDs;

            StackID = stackID;
        }
    }
}


