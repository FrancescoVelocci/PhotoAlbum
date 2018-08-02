using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

namespace PhotoAlbum.Controllers
{
    public class PlaceController : Controller
    {
        private readonly ApplicationDbContext context;

        public PlaceController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IList<Place> places = context.Places.ToList();

            return View(places);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddPlaceViewModel addPlaceViewModel = new AddPlaceViewModel();
            return View(addPlaceViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddPlaceViewModel addPlaceViewModel)
        {
            if (ModelState.IsValid)
            {
                Place newCategory = new Place
                {
                    Name = addPlaceViewModel.Name
                };

                context.Places.Add(newCategory);
                context.SaveChanges();

                return Redirect("/Place");
            }

            return View(addPlaceViewModel);
        }
    }
}
