using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;
using PhotoAlbum.Helpers;

// PICTURE Controller

namespace PhotoAlbum.Controllers
{
    public class PictureController : Controller
    {
        private ApplicationDbContext context;

        public PictureController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public List<ViewPictureHelper> SelectAllPictures()
        {
            var queryLeftJoinPicturesOnLocation =   (from p in context.Pictures
                                                    join pl in context.PictureLocations
                                                    on p.ID equals pl.PictureID into jj
                                                    from pl in jj.DefaultIfEmpty()
                                                    select new ViewPictureHelper
                                                    {
                                                        PictureID = p.ID,
                                                        PictureName = p.Name,
                                                        PictureDate = p.Date,
                                                        PictureFavorite = p.Favorite,
                                                        PictureStackID = p.StackID,
                                                        PictureStackIsClassified = p.Stack.Classified,

                                                        PictureNation = pl.Location.Nation,
                                                        PictureCity = pl.Location.City,
                                                        PicturePlaceType = pl.Location.Place.Name,
                                                        PicturePlaceName = pl.Location.PlaceName
                                                    }).ToList();

            var queryLeftJoinPicturesOnEvent = (from p in context.Pictures
                                                join pe in context.PictureEvents
                                                on p.ID equals pe.PictureID into jj
                                                from pe in jj.DefaultIfEmpty()
                                                select new ViewPictureHelper
                                                {
                                                     PictureID = p.ID,
                                                     PictureEventName = pe.Event.Name,
                                                     PictureEventType = pe.Event.EventType.Name
                                                }).ToList();

            var queryPictureLocationJoinEvent = (from pl in queryLeftJoinPicturesOnLocation
                                                join pe in queryLeftJoinPicturesOnEvent
                                                on pl.PictureID equals pe.PictureID
                                                select new ViewPictureHelper
                                                {
                                                    PictureID = pl.PictureID,
                                                    PictureName = pl.PictureName,
                                                    PictureDate = pl.PictureDate,
                                                    PictureFavorite = pl.PictureFavorite,
                                                    PictureStackID = pl.PictureStackID,
                                                    PictureStackIsClassified = pl.PictureStackIsClassified,

                                                    PictureNation = pl.PictureNation,
                                                    PictureCity = pl.PictureCity,
                                                    PicturePlaceType = pl.PicturePlaceType,
                                                    PicturePlaceName = pl.PicturePlaceName,

                                                    PictureEventName = pe.PictureEventName,
                                                    PictureEventType = pe.PictureEventType
                                                }).ToList();

            var queryLeftJoinPicturesOnAuthor = (from p in context.Pictures
                                                 join pa in context.PictureAuthors
                                                 on p.ID equals pa.PictureID into JJ
                                                 from pa in JJ.DefaultIfEmpty()
                                                 select new ViewPictureHelper
                                                 {
                                                     PictureID = p.ID,
                                                     PictureAuthorName = pa.Author.Name,
                                                     PictureAuthorLastName = pa.Author.LastName
                                                 }).ToList();

            var queryPictureLocationEventJoinAuthor = (from ple in queryPictureLocationJoinEvent
                                                      join pa in queryLeftJoinPicturesOnAuthor
                                                      on ple.PictureID equals pa.PictureID
                                                      select new ViewPictureHelper
                                                      {
                                                          PictureID = ple.PictureID,
                                                          PictureName = ple.PictureName,
                                                          PictureDate = ple.PictureDate,
                                                          PictureFavorite = ple.PictureFavorite,
                                                          PictureStackID = ple.PictureStackID,
                                                          PictureStackIsClassified = ple.PictureStackIsClassified,

                                                          PictureNation = ple.PictureNation,
                                                          PictureCity = ple.PictureCity,
                                                          PicturePlaceType = ple.PicturePlaceType,
                                                          PicturePlaceName = ple.PicturePlaceName,

                                                          PictureEventName = ple.PictureEventName,
                                                          PictureEventType = ple.PictureEventType,

                                                          PictureAuthorName = pa.PictureAuthorName,
                                                          PictureAuthorLastName = pa.PictureAuthorLastName
                                                      }).ToList();

            var queryLeftJoinPicturesOnPeople = (from p in context.Pictures
                                                 join pp in context.PicturePeopleDb
                                                 on p.ID equals pp.PictureID into JJ
                                                 from pp in JJ.DefaultIfEmpty()
                                                 select new ViewPictureHelper
                                                 {
                                                     PictureID = p.ID,
                                                     PicturePeopleName = pp.People.Name,
                                                     PicturePeopleLastName = pp.People.LastName
                                                 })
                                                 .ToList();

            var querySelectAll = (from plea in queryPictureLocationEventJoinAuthor
                                  join pp in queryLeftJoinPicturesOnPeople
                                  on plea.PictureID equals pp.PictureID
                                  select new ViewPictureHelper
                                  {
                                      PictureID = plea.PictureID,
                                      PictureName = plea.PictureName,
                                      PictureDate = plea.PictureDate,
                                      PictureFavorite = plea.PictureFavorite,
                                      PictureStackID = plea.PictureStackID,
                                      PictureStackIsClassified = plea.PictureStackIsClassified,

                                      PictureNation = plea.PictureNation,
                                      PictureCity = plea.PictureCity,
                                      PicturePlaceType = plea.PicturePlaceType,
                                      PicturePlaceName = plea.PicturePlaceName,

                                      PictureEventName = plea.PictureEventName,
                                      PictureEventType = plea.PictureEventType,

                                      PictureAuthorName = plea.PictureAuthorName,
                                      PictureAuthorLastName = plea.PictureAuthorLastName,

                                      PicturePeopleName = pp.PicturePeopleName,
                                      PicturePeopleLastName = pp.PicturePeopleLastName
                                  }).ToList();

            return querySelectAll;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            ViewBag.DateSortParmDesc = sortOrder == "Date" ? "date_desc" : "DateDesc";
            ViewBag.DateSortParmAsc = sortOrder == "Date" ? "date_asc" : "DateAsc";

            var selectAllPictures = SelectAllPictures();

            IQueryable<ViewPictureHelper> viewPictureHelper = selectAllPictures.AsQueryable();

            switch (sortOrder)
            {
                case "DateDesc":
                    viewPictureHelper = viewPictureHelper.OrderByDescending(pic => pic.PictureDate);
                    break;

                case "DateAsc":
                    viewPictureHelper = viewPictureHelper.OrderBy(pic => pic.PictureDate);
                    break;
            }

            ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(viewPictureHelper.ToList());

            return View(viewPictureViewModel);
        }

        [HttpGet]
        public IActionResult Stack(string ID, string sortOrder)
        {
            ViewBag.DateSortParmDesc = sortOrder == "Date" ? "date_desc" : "DateDesc";
            ViewBag.DateSortParmAsc = sortOrder == "Date" ? "date_asc" : "DateAsc";
            ViewBag.StackID = ID;

            var selectAllPictures = SelectAllPictures();

            if (ID == null)
            {
                var selectAllPictureOfAStack = selectAllPictures.AsQueryable();

                switch (sortOrder)
                {
                    case "DateDesc":
                        selectAllPictureOfAStack = selectAllPictureOfAStack
                            .OrderByDescending(pic => pic.PictureDate);
                        break;

                    case "DateAsc":
                        selectAllPictureOfAStack = selectAllPictureOfAStack
                            .OrderBy(pic => pic.PictureDate);
                        break;
                }

                ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(selectAllPictureOfAStack.ToList());

                return View(viewPictureViewModel);
            }

            else
            {
                var selectAllPictureOfAStack = selectAllPictures
                    .Where(p => p.PictureStackID == int.Parse(ID))
                    .AsQueryable();

                switch (sortOrder)
                {
                    case "DateDesc":
                        selectAllPictureOfAStack = selectAllPictureOfAStack.OrderByDescending(pic => pic.PictureDate);
                        break;

                    case "DateAsc":
                        selectAllPictureOfAStack = selectAllPictureOfAStack.OrderBy(pic => pic.PictureDate);
                        break;
                }

                ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(selectAllPictureOfAStack.ToList());

                return View(viewPictureViewModel);
            }
        }

        public List<ViewPictureHelper> SelectAllPicturesWhithoutLocation()
        {
            var querySelectAllPicturesWhithoutLocation = (from p in context.Pictures
                                                            join pl in context.PictureLocations
                                                            on p.ID equals pl.PictureID into jj
                                                            from pl in jj.DefaultIfEmpty()
                                                            where pl.LocationID.ToString() == null
                                                            select new ViewPictureHelper
                                                            {
                                                                PictureID = p.ID,
                                                                PictureName = p.Name,
                                                                PictureDate = p.Date,
                                                                PictureFavorite = p.Favorite,
                                                                PictureStackID = p.StackID,
                                                                PictureStackIsClassified = p.Stack.Classified,

                                                                PictureNation = pl.Location.Nation,
                                                                PictureCity = pl.Location.City,
                                                                PicturePlaceType = pl.Location.Place.Name,
                                                                PicturePlaceName = pl.Location.PlaceName
                                                            })
                                                            .Distinct()
                                                            .ToList();

            return querySelectAllPicturesWhithoutLocation;
        }

        [HttpGet]
        public IActionResult AddPictureLocation(string stackID, string sortOrder)
        {
            ViewBag.DateSortParmDesc = sortOrder == "Date" ? "date_desc" : "DateDesc";
            ViewBag.DateSortParmAsc = sortOrder == "Date" ? "date_asc" : "DateAsc";
            ViewBag.StackID = stackID;

            var selectAllPicturesWithoutLocation = SelectAllPicturesWhithoutLocation();

            var slectAllPictureWithoutLocationNotYetClassified = selectAllPicturesWithoutLocation
                                                                .Where(p => p.PictureStackIsClassified == false)
                                                                .ToList();

            if (stackID != null)
            {
                var selectAllPictureOfAStackWithoutLocation = slectAllPictureWithoutLocationNotYetClassified
                                                            .Where(p => p.PictureStackID == int.Parse(stackID))
                                                            .AsQueryable();

                switch (sortOrder)
                {
                    case "DateDesc":
                        selectAllPictureOfAStackWithoutLocation = selectAllPictureOfAStackWithoutLocation
                            .OrderByDescending(pic => pic.PictureDate);
                        break;

                    case "DateAsc":
                        selectAllPictureOfAStackWithoutLocation = selectAllPictureOfAStackWithoutLocation
                            .OrderBy(pic => pic.PictureDate);
                        break;
                }

                string _stackID = stackID;

                var locations = context.Locations.Include(p => p.Place).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutLocation.Select(p => p.PictureID).ToList();

                AddPictureLocationViewModel addPictureLocationViewModel =
                                                    new AddPictureLocationViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutLocation.ToList(),
                                                                        locations,
                                                                        pictureIDs,
                                                                        _stackID
                                                                        );

                return View(addPictureLocationViewModel);
            }

            else
            {
                var selectAllPictureOfAStackWithoutLocation = slectAllPictureWithoutLocationNotYetClassified
                                                            .AsQueryable();

                switch (sortOrder)
                {
                    case "DateDesc":
                        selectAllPictureOfAStackWithoutLocation = selectAllPictureOfAStackWithoutLocation
                            .OrderByDescending(pic => pic.PictureDate);
                        break;

                    case "DateAsc":
                        selectAllPictureOfAStackWithoutLocation = selectAllPictureOfAStackWithoutLocation
                            .OrderBy(pic => pic.PictureDate);
                        break;
                }

                string _stackID = stackID;

                var locations = context.Locations.Include(p => p.Place).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutLocation.Select(p => p.PictureID).ToList();

                AddPictureLocationViewModel addPictureLocationViewModel =
                                                    new AddPictureLocationViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutLocation.ToList(),
                                                                        locations,
                                                                        pictureIDs,
                                                                        _stackID
                                                                        );

                return View(addPictureLocationViewModel);
            }
        }

        [HttpPost]
        public IActionResult AddPictureLocation(AddPictureLocationViewModel addPictureLocationViewModel)
        {
            if (ModelState.IsValid)
            {
                string _stackID = addPictureLocationViewModel.StackID;

                int locationID = addPictureLocationViewModel.LocationID;

                List<int> pictureIDs = addPictureLocationViewModel.PictureIDs;

                foreach (var pictureID in pictureIDs)
                {
                    PictureLocation pictureLocation = new PictureLocation()
                    {
                        PictureID = pictureID,
                        LocationID = locationID
                    };

                    context.PictureLocations.Add(pictureLocation);
                }

                context.SaveChanges();

                return Redirect(string.Format("/Picture/AddPIctureLocation/StackID={0}", _stackID));
            }

            return View(addPictureLocationViewModel);
        }
    }
}
