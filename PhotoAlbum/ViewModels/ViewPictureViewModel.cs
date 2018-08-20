using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoAlbum.Models;
using PhotoAlbum.Helpers;

namespace PhotoAlbum.ViewModels
{
    public class ViewPictureViewModel
    {
        public List<ViewPictureHelper> ViewPictureHelpers { get; set; }

        public ViewPictureViewModel()
        { }

        public ViewPictureViewModel(List<ViewPictureHelper> viewPictureHelpers)
        {
            ViewPictureHelpers = viewPictureHelpers;
        }
    }
}
