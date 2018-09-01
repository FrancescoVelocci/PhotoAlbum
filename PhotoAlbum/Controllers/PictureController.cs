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
            var peopleList = context.Pictures.Select(p => new { p.ID, p.People, }).ToList();

            List<PicturePeopleHelper> listOfPicturePeopleIdsByPictureId = new List<PicturePeopleHelper>();

            foreach (var picturePeopleIDs in peopleList)
            {
                if (picturePeopleIDs.People == null)
                {
                    continue;
                }

                else
                {
                    List<string> picturePeopleId = picturePeopleIDs.People.Split(',').ToList();
                    var a = new PicturePeopleHelper
                    {
                        PictureID = picturePeopleIDs.ID,
                        PeopleIDs = picturePeopleId
                    };
                    listOfPicturePeopleIdsByPictureId.Add(a);
                }
            };

            List<ViewPictureHelper> pictureIdandPeopleId = new List<ViewPictureHelper>();

            foreach (var viewPicturePeopleObj in listOfPicturePeopleIdsByPictureId)
            {
                foreach (var idString in viewPicturePeopleObj.PeopleIDs)
                {
                    var viewPictureHelperObj = new ViewPictureHelper
                    {
                        PictureID = viewPicturePeopleObj.PictureID,
                        PicturePeopleIDs = idString
                    };
                    pictureIdandPeopleId.Add(viewPictureHelperObj);
                };
            };

            var pictureIdandPeopleName = (from p in pictureIdandPeopleId
                                          join pn in context.PeopleDb
                                          on int.Parse(p.PicturePeopleIDs) equals pn.ID
                                          select new ViewPictureHelper
                                          {
                                              PictureID = p.PictureID,
                                              PicturePeopleIDs = pn.Name + " " + pn.LastName
                                          }).ToList();
            
            var groupedID = (from g in pictureIdandPeopleName
                             group g by g.PictureID into jj
                             select new ViewPictureHelper
                             {
                                 PictureID = jj.First().PictureID,
                                 PicturePeopleIDs = string.Join(", ", jj.Select(p => p.PicturePeopleIDs))
                             }).ToList();

            var queryPicturesLeftJoinPeopleNames = (from p in context.Pictures
                                                    join pp in groupedID
                                                    on p.ID equals pp.PictureID into jj
                                                    from pp in jj.DefaultIfEmpty()
                                                    select new ViewPictureHelper
                                                    {
                                                        PictureID = p.ID,
                                                        PictureName = p.Name,
                                                        PictureDate = p.Date,
                                                        PictureFavorite = p.Favorite,
                                                        PictureStackID = p.StackID,
                                                        PictureStackIsClassified = p.Stack.Classified,
                                                        PicturePeopleIDs = pp.PicturePeopleIDs,
                                                    }).ToList();
                       
            var queryLeftJoinPicturesOnLocation = (from p in context.Pictures
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

            var queryPictureWithNameseJoinPictureLocation = (from pl in queryLeftJoinPicturesOnLocation
                                                             join pp in queryPicturesLeftJoinPeopleNames
                                                             on pl.PictureID equals pp.PictureID into jj
                                                             from pp in jj.DefaultIfEmpty()
                                                             select new ViewPictureHelper
                                                             {
                                                                 PictureID = pp.PictureID,
                                                                 PictureName = pp.PictureName,
                                                                 PictureDate = pp.PictureDate,
                                                                 PictureFavorite = pp.PictureFavorite,
                                                                 PictureStackID = pp.PictureStackID,
                                                                 PictureStackIsClassified = pp.PictureStackIsClassified,
                                                                 PicturePeopleIDs = pp.PicturePeopleIDs,

                                                                 PictureNation = pl.PictureNation,
                                                                 PictureCity = pl.PictureCity,
                                                                 PicturePlaceType = pl.PicturePlaceType,
                                                                 PicturePlaceName = pl.PicturePlaceName,
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

            var queryPictureLocationJoinEvent = (from pl in queryPictureWithNameseJoinPictureLocation
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
                                                     PicturePeopleIDs = pl.PicturePeopleIDs,

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

            var querySelectAll = (from ple in queryPictureLocationJoinEvent
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
                                      PicturePeopleIDs = ple.PicturePeopleIDs,

                                      PictureNation = ple.PictureNation,
                                      PictureCity = ple.PictureCity,
                                      PicturePlaceType = ple.PicturePlaceType,
                                      PicturePlaceName = ple.PicturePlaceName,

                                      PictureEventName = ple.PictureEventName,
                                      PictureEventType = ple.PictureEventType,

                                      PictureAuthorName = pa.PictureAuthorName,
                                      PictureAuthorLastName = pa.PictureAuthorLastName
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

            var pictureID = SelectAllPictures().Select(p => p.PictureID).First();
            var isFavorite = true;

            ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(viewPictureHelper.ToList(), pictureID, isFavorite);

            return View(viewPictureViewModel);
        }

        [HttpPost]
        public IActionResult Index(ViewPictureViewModel viewPictureViewModel)
        {
            if (ModelState.IsValid)
            {
                var pictureID = viewPictureViewModel.PictureID;
                var picture = new Picture { ID = pictureID };

                picture.Favorite = viewPictureViewModel.IsFavorite;
                context.Entry(picture).Property("Favorite").IsModified = true;
                context.SaveChanges();
                
                return Redirect("/Picture");
            }          

            return View(viewPictureViewModel);
        }
        
        [HttpGet]
        public IActionResult Stack(string ID, string sortOrder)
        {
            ViewBag.DateSortParmDesc = sortOrder == "Date" ? "date_desc" : "DateDesc";
            ViewBag.DateSortParmAsc = sortOrder == "Date" ? "date_asc" : "DateAsc";
            ViewBag.StackID = ID;

            var selectAllPictures = SelectAllPictures();

            var pictureID = SelectAllPictures().Select(p => p.PictureID).First();
            var isFavorite = true;

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

                ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(selectAllPictureOfAStack.ToList(),
                                                                                    pictureID,
                                                                                    isFavorite);

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

                ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(selectAllPictureOfAStack.ToList(),
                                                                                    pictureID,
                                                                                    isFavorite);

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

        public List<ViewPictureHelper> SelectAllPicturesWhithoutEvent()
        {
            var querySelectAllPicturesWhithoutEvent = (from p in context.Pictures
                                                       join pe in context.PictureEvents
                                                       on p.ID equals pe.PictureID into jj
                                                       from pe in jj.DefaultIfEmpty()
                                                       where pe.EventID.ToString() == null
                                                       select new ViewPictureHelper
                                                       {
                                                           PictureID = p.ID,
                                                           PictureName = p.Name,
                                                           PictureDate = p.Date,
                                                           PictureFavorite = p.Favorite,
                                                           PictureStackID = p.StackID,
                                                           PictureStackIsClassified = p.Stack.Classified,

                                                           PictureEventName = pe.Event.Name,
                                                           PictureEventType = pe.Event.EventType.Name
                                                       })
                                                          .Distinct()
                                                          .ToList();

            return querySelectAllPicturesWhithoutEvent;
        }

        [HttpGet]
        public IActionResult AddPictureEvent(string stackID)
        {
            var selectAllPicturesWithoutEvent = SelectAllPicturesWhithoutEvent();

            var slectAllPictureWithoutEventNotYetClassified = selectAllPicturesWithoutEvent
                                                                .Where(p => p.PictureStackIsClassified == false)
                                                                .ToList();

            if (stackID != null)
            {
                var selectAllPictureOfAStackWithoutEvent = slectAllPictureWithoutEventNotYetClassified
                                                            .Where(p => p.PictureStackID == int.Parse(stackID))
                                                            .AsQueryable();

                string _stackID = stackID;

                var events = context.Events.Include(p => p.EventType).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutEvent.Select(p => p.PictureID).ToList();

                AddPictureEventViewModel addPictureEventViewModel =
                                                    new AddPictureEventViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutEvent.ToList(),
                                                                        events,
                                                                        pictureIDs,
                                                                        _stackID
                                                                        );

                return View(addPictureEventViewModel);
            }

            else
            {
                var selectAllPictureOfAStackWithoutEvent = slectAllPictureWithoutEventNotYetClassified
                                                            .AsQueryable();

                string _stackID = stackID;

                var events = context.Events.Include(p => p.EventType).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutEvent.Select(p => p.PictureID).ToList();

                AddPictureEventViewModel addPictureEventViewModel =
                                                    new AddPictureEventViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutEvent.ToList(),
                                                                        events,
                                                                        pictureIDs,
                                                                        _stackID
                                                                        );

                return View(addPictureEventViewModel);
            }
        }

        [HttpPost]
        public IActionResult AddPictureEvent(AddPictureEventViewModel addPictureEventViewModel)
        {
            if (ModelState.IsValid)
            {
                string _stackID = addPictureEventViewModel.StackID;

                int eventID = addPictureEventViewModel.EventID;

                List<int> pictureIDs = addPictureEventViewModel.PictureIDs;

                foreach (var pictureID in pictureIDs)
                {
                    PictureEvent pictureEvent = new PictureEvent()
                    {
                        PictureID = pictureID,
                        EventID = eventID
                    };

                    context.PictureEvents.Add(pictureEvent);
                }

                context.SaveChanges();

                return Redirect(string.Format("/Picture/AddPIctureEvent/StackID={0}", _stackID));
            }

            return View(addPictureEventViewModel);
        }

        public List<ViewPictureHelper> SelectAllPicturesWhithoutAuthor()
        {
            var querySelectAllPicturesWhithoutAuthor = (from p in context.Pictures
                                                       join pa in context.PictureAuthors
                                                       on p.ID equals pa.PictureID into jj
                                                       from pa in jj.DefaultIfEmpty()
                                                       where pa.AuthorID.ToString() == null
                                                       select new ViewPictureHelper
                                                       {
                                                           PictureID = p.ID,
                                                           PictureName = p.Name,
                                                           PictureDate = p.Date,
                                                           PictureFavorite = p.Favorite,
                                                           PictureStackID = p.StackID,
                                                           PictureStackIsClassified = p.Stack.Classified,

                                                           PictureAuthorName = pa.Author.Name,
                                                           PictureAuthorLastName = pa.Author.LastName
                                                       })
                                                          .Distinct()
                                                          .ToList();

            return querySelectAllPicturesWhithoutAuthor;
        }

        [HttpGet]
        public IActionResult AddPictureAuthor(string stackID)
        {
            var selectAllPicturesWithoutAuthor = SelectAllPicturesWhithoutAuthor();

            var slectAllPictureWithoutAuthorNotYetClassified = selectAllPicturesWithoutAuthor
                                                                .Where(p => p.PictureStackIsClassified == false)
                                                                .ToList();

            if (stackID != null)
            {
                var selectAllPictureOfAStackWithoutAuthor = slectAllPictureWithoutAuthorNotYetClassified
                                                            .Where(p => p.PictureStackID == int.Parse(stackID))
                                                            .AsQueryable();

                string _stackID = stackID;

                var authors = context.Authors.ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutAuthor.Select(p => p.PictureID).ToList();

                AddPictureAuthorViewModel addPictureAuthorViewModel =
                                                    new AddPictureAuthorViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutAuthor.ToList(),
                                                                        authors,
                                                                        pictureIDs,
                                                                        _stackID
                                                                        );

                return View(addPictureAuthorViewModel);
            }

            else
            {
                var selectAllPictureOfAStackWithoutAuthor = slectAllPictureWithoutAuthorNotYetClassified
                                                            .AsQueryable();

                string _stackID = stackID;

                var authors = context.Authors.ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutAuthor.Select(p => p.PictureID).ToList();

                AddPictureAuthorViewModel addPictureAuthorViewModel =
                                                    new AddPictureAuthorViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutAuthor.ToList(),
                                                                        authors,
                                                                        pictureIDs,
                                                                        _stackID
                                                                        );

                return View(addPictureAuthorViewModel);
            }
        }

        [HttpPost]
        public IActionResult AddPictureAuthor(AddPictureAuthorViewModel addPictureAuthorViewModel)
        {
            if (ModelState.IsValid)
            {
                string _stackID = addPictureAuthorViewModel.StackID;

                int authorID = addPictureAuthorViewModel.AuthorID;

                List<int> pictureIDs = addPictureAuthorViewModel.PictureIDs;

                foreach (var pictureID in pictureIDs)
                {
                    PictureAuthor pictureAuthor = new PictureAuthor()
                    {
                        PictureID = pictureID,
                        AuthorID = authorID
                    };

                    context.PictureAuthors.Add(pictureAuthor);
                }

                context.SaveChanges();

                return Redirect(string.Format("/Picture/AddPIctureAuthor/StackID={0}", _stackID));
            }

            return View(addPictureAuthorViewModel);
        }
    
        public List<ViewPictureHelper> SelectAllPicturesWhithoutPeople()
        {
            var querySelectAllPicturesWhithoutPeople = (from p in SelectAllPictures()
                                                        where p.PicturePeopleIDs == null
                                                        select new ViewPictureHelper
                                                        {
                                                            PictureID = p.PictureID,
                                                            PictureName = p.PictureName,
                                                            PictureDate = p.PictureDate,
                                                            PictureFavorite = p.PictureFavorite,
                                                            PictureStackID = p.PictureStackID,
                                                            PictureStackIsClassified = p.PictureStackIsClassified,

                                                            PicturePeopleIDs = p.PicturePeopleIDs
                                                        })
                                                          .Distinct()
                                                          .ToList();

            return querySelectAllPicturesWhithoutPeople;
        }
    
        [HttpGet]
        public IActionResult AddPicturePeople(string stackID)
        {
            var selectAllPicturesWithoutPeople = SelectAllPicturesWhithoutPeople();

            var slectAllPictureWithoutPeopleNotYetClassified = selectAllPicturesWithoutPeople
                                                                .Where(p => p.PictureStackIsClassified == false)
                                                                .ToList();

            if (stackID != null)
            {
                var selectAllPictureOfAStackWithoutPeople = slectAllPictureWithoutPeopleNotYetClassified
                                                            .Where(p => p.PictureStackID == int.Parse(stackID))
                                                            .AsQueryable();

                string _stackID = stackID;

                var peopleNameList = SelectAllPictures().Select(p => p.PicturePeopleIDs).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutPeople.Select(p => p.PictureID).ToList();

                var peopleList = context.PeopleDb.ToList();

                List<int> peopleIDs = new List<int>();

                AddPicturePeopleViewModel addPicturePeopleViewModel =
                                                    new AddPicturePeopleViewModel
                                                                (
                                                                selectAllPictureOfAStackWithoutPeople.ToList(),
                                                                peopleNameList,
                                                                pictureIDs,
                                                                _stackID,
                                                                peopleList,
                                                                peopleIDs
                                                                );

                return View(addPicturePeopleViewModel);
            }

            else
            {
                var selectAllPictureOfAStackWithoutAuthor = slectAllPictureWithoutPeopleNotYetClassified
                                                            .AsQueryable();

                string _stackID = stackID;

                var peopleNameList = SelectAllPictures().Select(p => p.PicturePeopleIDs).ToList();

                var pictureIDs = slectAllPictureWithoutPeopleNotYetClassified.Select(p => p.PictureID).ToList();

                var peopleList = context.PeopleDb.ToList();

                List<int> peopleIDs = new List<int>();

                AddPicturePeopleViewModel addPicturePeopleViewModel =
                                                    new AddPicturePeopleViewModel
                                                                (
                                                                slectAllPictureWithoutPeopleNotYetClassified.ToList(),
                                                                peopleNameList,
                                                                pictureIDs,
                                                                _stackID,
                                                                peopleList,
                                                                peopleIDs
                                                                );

                return View(addPicturePeopleViewModel);
                }
        }
    
        [HttpPost]
        public IActionResult AddPicturePeople(AddPicturePeopleViewModel addPicturePeopleViewModel)
        {
            if (ModelState.IsValid)
            {
                string _stackID = addPicturePeopleViewModel.StackID;

                List<int> pictureIDs = addPicturePeopleViewModel.PictureIDs;

                List<string> peopleIDList = new List<string>(); 
                foreach (var c in addPicturePeopleViewModel.PeopleIDs)
                    {
                        var id = c.ToString();
                        peopleIDList.Add(id);
                    };

                string peopleIDs = string.Join(", ", peopleIDList);

                foreach (var pictureID in pictureIDs)
                {
                        var picture = new Picture { ID = pictureID };
                        picture.People = peopleIDs;
                        context.Entry(picture).Property("People").IsModified = true;
                };

                context.SaveChanges();

                return Redirect(string.Format("/Picture/AddPIcturePeople/StackID={0}", _stackID));
            }

            return View(addPicturePeopleViewModel);
        }

    }
}

