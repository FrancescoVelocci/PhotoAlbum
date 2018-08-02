using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

namespace PhotoAlbum.Controllers
{
    public class LocationController : Controller
    {
        // GET: /<controller>/
        private ApplicationDbContext context;

        public LocationController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IList<Location> locations = context.Locations.Include(c => c.Place).ToList();

            return View(locations);
        }

        [HttpGet]
        public IActionResult Add()
        {
            IList<Place> places = context.Places.ToList();
            AddLocationViewModel addLocationViewModel = new AddLocationViewModel(places);
            return View(addLocationViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddLocationViewModel addLocationViewModel)
        {
            if (ModelState.IsValid)
            {
                Place newPlace = context.Places.Single(c => c.ID == addLocationViewModel.PlaceID);

                Location newLocation = new Location
                {
                    Nation = addLocationViewModel.Nation,
                    City = addLocationViewModel.City,
                    Place = newPlace,
                    PlaceName = addLocationViewModel.PlaceName
                };

                context.Locations.Add(newLocation);
                context.SaveChanges();

                return Redirect("/Location");
            }

            return View(addLocationViewModel);
        }
    }
}
