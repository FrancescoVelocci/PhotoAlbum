using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Models;
using PhotoAlbum.Helpers;

namespace PhotoAlbum.ViewModels
{
    public class ViewPictureStackViewModel : ViewPictureViewModel
    {
        public string ID { get; set; }

        public ViewPictureStackViewModel()
        { }

        public ViewPictureStackViewModel(
                                List<ViewPictureHelper> viewPictureHelpers,
                                int pictureID,
                                bool isFavorite,
                                string id)
        {
            ViewPictureHelpers = viewPictureHelpers;

            PictureID = pictureID;

            IsFavorite = isFavorite;

            ID = id;
        }

    }
}
