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

        private DataProvider provider;

        public PictureController(ApplicationDbContext dbContext)
        {
            provider = new DataProvider(dbContext);
            context = dbContext;
        }
 
        public IQueryable<ViewPictureHelper> Sorting(string sortOrder, IQueryable<ViewPictureHelper> viewPictureHelper)
        {
            if (sortOrder == "Desc")
            {
                var viewPictureHelperSorted = viewPictureHelper.OrderByDescending(p => p.PictureDate);
                return (viewPictureHelperSorted);
            }

            else
            {
                var viewPictureHelperSorted = viewPictureHelper.OrderBy(p => p.PictureDate);
                return (viewPictureHelperSorted);
            }
        }

        [HttpGet]
        public IActionResult Index(string sortOrder = "Asc")
        {
            ViewBag.Sorting = sortOrder;

            IQueryable<ViewPictureHelper> selectAllPictures = provider.SelectAllPictures().AsQueryable();

            var pictureID = provider.SelectAllPictures().Select(p => p.PictureID).First();
            var isFavorite = true;

            var viewPictureHelperSorted = Sorting(sortOrder, selectAllPictures);
            ViewPictureViewModel viewPictureViewModel = new ViewPictureViewModel(
                                                                    viewPictureHelperSorted.ToList(),
                                                                    pictureID,
                                                                    isFavorite);

            return View(viewPictureViewModel);  
        }

        public Picture SetFavorite(ViewPictureViewModel viewPictureViewModel)
        {
            var pictureID = viewPictureViewModel.PictureID;
            var picture = new Picture { ID = pictureID };

            picture.Favorite = viewPictureViewModel.IsFavorite;

            return (picture);
        }

        [HttpPost]
        public IActionResult Index(ViewPictureViewModel viewPictureViewModel)
        {
            if (ModelState.IsValid)
            {
                Picture picture = SetFavorite(viewPictureViewModel);

                context.Entry(picture).Property("Favorite").IsModified = true;
                context.SaveChanges();
                
                return Redirect("/Picture");
            }          

            return View(viewPictureViewModel);
        }
        
        [HttpGet]
        public IActionResult Stack([FromQuery]string ID, [FromQuery]string sortOrder = "Asc")
        {
            ViewBag.Sorting = sortOrder;
            ViewBag.StackID = ID;

            var pictureID = provider.SelectAllPictures().Select(p => p.PictureID).First();
            var isFavorite = true;

            if (ID != null)
            {
                var selectAllPictureOfAStack = provider.SelectAllPictures().AsQueryable();

                var viewPictureHelperSorted = Sorting(sortOrder, selectAllPictureOfAStack)
                                                    .Where(p => p.PictureStackID == int.Parse(ID))
                                                    .Where(p => p.PictureStackIsClassified == false);

                ViewPictureStackViewModel viewPictureStackViewModel = new ViewPictureStackViewModel(
                                                                        viewPictureHelperSorted.ToList(),
                                                                        pictureID,
                                                                        isFavorite,
                                                                        ID);

                return View(viewPictureStackViewModel);
            }

            else
            {
                var selectAllPicturesStillInStack = provider.SelectAllPictures().AsQueryable();

                var viewPictureHelperSorted = Sorting(sortOrder, selectAllPicturesStillInStack)
                                                    
                                                .Where(p => p.PictureStackIsClassified == false);

                ViewPictureStackViewModel viewPictureStackViewModel = new ViewPictureStackViewModel(
                                                                        viewPictureHelperSorted.ToList(),
                                                                        pictureID,
                                                                        isFavorite,
                                                                        ID);

                return View(viewPictureStackViewModel);
            }
        }

        [HttpPost]
        public IActionResult Stack(ViewPictureViewModel viewPictureViewModel, string ID)
        {
            if (ModelState.IsValid)
            {
                Picture picture = SetFavorite(viewPictureViewModel);
                context.Entry(picture).Property("Favorite").IsModified = true;
                context.SaveChanges();

                if (ID == null)
                {
                    return Redirect("/Picture/Stack?ID=");
                }

                else
                {
                    return Redirect(string.Format("/Picture/Stack?ID={0}", ID));
                }
            }

            return View(viewPictureViewModel);
        }

        [HttpGet]
        public IActionResult AddPictureLocation(string stackID, string sortOrder)
        {
            ViewBag.Sorting = sortOrder;

            var selectAllPicturesWithoutLocation = provider.SelectAllPicturesWhithoutLocation();

            var slectAllPictureWithoutLocationNotYetClassified = selectAllPicturesWithoutLocation
                                                                .Where(p => p.PictureStackIsClassified == false)
                                                                .AsQueryable();

            if (stackID != null)
            {
                var selectAllPictureOfAStackWithoutLocation = Sorting(sortOrder, slectAllPictureWithoutLocationNotYetClassified)
                                                            .Where(p => p.PictureStackID == int.Parse(stackID))
                                                            .AsQueryable();

                var locations = context.Locations.Include(p => p.Place).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutLocation.Select(p => p.PictureID).ToList();

                AddPictureLocationViewModel addPictureLocationViewModel =
                                                    new AddPictureLocationViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutLocation.ToList(),
                                                                        locations,
                                                                        pictureIDs,
                                                                        stackID
                                                                        );

                return View(addPictureLocationViewModel);
            }

            else
            {
                var selectAllPictureOfAStackWithoutLocation = Sorting(sortOrder, slectAllPictureWithoutLocationNotYetClassified)
                                                            .AsQueryable();

                var locations = context.Locations.Include(p => p.Place).ToList();

                var pictureIDs = selectAllPictureOfAStackWithoutLocation.Select(p => p.PictureID).ToList();

                AddPictureLocationViewModel addPictureLocationViewModel =
                                                    new AddPictureLocationViewModel
                                                                        (
                                                                        selectAllPictureOfAStackWithoutLocation.ToList(),
                                                                        locations,
                                                                        pictureIDs,
                                                                        stackID
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

                return Redirect(string.Format("/Picture/AddPIctureLocation?StackID={0}", _stackID));
            }

            return View(addPictureLocationViewModel);
        }

        [HttpGet]
        public IActionResult AddPictureEvent(string stackID)
        {
            var selectAllPicturesWithoutEvent = provider.SelectAllPicturesWhithoutEvent();

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

                return Redirect(string.Format("/Picture/AddPIctureEvent?StackID={0}", _stackID));
            }

            return View(addPictureEventViewModel);
        }

        [HttpGet]
        public IActionResult AddPictureAuthor(string stackID)
        {
            var selectAllPicturesWithoutAuthor = provider.SelectAllPicturesWhithoutAuthor();

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

                return Redirect(string.Format("/Picture/AddPIctureAuthor?StackID={0}", _stackID));
            }

            return View(addPictureAuthorViewModel);
        }
    
        [HttpGet]
        public IActionResult AddPicturePeople(string stackID)
        {
            var selectAllPicturesWithoutPeople = provider.SelectAllPicturesWhithoutPeople();

            var slectAllPictureWithoutPeopleNotYetClassified = selectAllPicturesWithoutPeople
                                                                .Where(p => p.PictureStackIsClassified == false)
                                                                .ToList();

            if (stackID != null)
            {
                var selectAllPictureOfAStackWithoutPeople = slectAllPictureWithoutPeopleNotYetClassified
                                                            .Where(p => p.PictureStackID == int.Parse(stackID))
                                                            .AsQueryable();

                string _stackID = stackID;

                var peopleNameList = provider.SelectAllPictures().Select(p => p.PicturePeopleIDs).ToList();

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

                var peopleNameList = provider.SelectAllPictures().Select(p => p.PicturePeopleIDs).ToList();

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

                return Redirect(string.Format("/Picture/AddPIcturePeople?StackID={0}", _stackID));
            }

            return View(addPicturePeopleViewModel);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

    }
}

