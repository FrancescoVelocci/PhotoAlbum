using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Helpers;

namespace PhotoAlbum.ViewModels
{
    public class ViewPictureAlbumViewModel
    {
        public List<ViewPictureHelper> ViewPictureHelpers { get; set; }
        public int AlbumID { get; set; }

        public ViewPictureAlbumViewModel()
        { }

        public ViewPictureAlbumViewModel(List<ViewPictureHelper> viewPictureHelpers, int albumID)
        {
            ViewPictureHelpers = viewPictureHelpers;

            AlbumID = albumID;
        }
    }
}
