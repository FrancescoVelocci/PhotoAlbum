using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;
using PhotoAlbum.Helpers;
using Microsoft.EntityFrameworkCore;

// ALBUM Controller

namespace PhotoAlbum.Controllers
{
    public class AlbumController : Controller
    {
        private ApplicationDbContext context;

        private DataProvider provider;

        public AlbumController(ApplicationDbContext dbContext)
        {
            provider = new DataProvider(dbContext);
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Album> albums = context.Albums.ToList();

            return View(albums);
        }

        [HttpGet]
        public IActionResult AddAlbum()
        {
            AddAlbumViewModel addAlbumViewModel = new AddAlbumViewModel();
            
            return View(addAlbumViewModel);
        }

        [HttpPost]
        public IActionResult AddAlbum(AddAlbumViewModel addAlbumViewModel)
        {
            if (ModelState.IsValid)
            {
                DateTime creationDate = DateTime.Now;

                Album album = new Album()
                {
                    Name = addAlbumViewModel.Name,
                    Description = addAlbumViewModel.Description,
                    DateTime = creationDate
                };

                context.Albums.Add(album);
                context.SaveChanges();

                return Redirect("/Album");
            }

            return View(addAlbumViewModel);
        }

        [HttpGet]
        public IActionResult View(int ID)
        {
            int _albumID = ID;

            ViewBag.AlbumName = context.Albums.Where(a => a.ID == _albumID).Select(a => a.Name).ToList();

            var query = (from a in provider.SelectAllPictures()
                         join b in context.PictureXAlbums
                         on a.PictureID equals b.PictureID
                         select new ViewPictureHelper
                         {
                             PictureID = a.PictureID,
                             PictureName = a.PictureName,
                             PictureDate = a.PictureDate,
                             PictureFavorite = a.PictureFavorite,
                             PictureStackID = a.PictureStackID,
                             PictureStackIsClassified = a.PictureStackIsClassified,
                             PicturePeopleIDs = a.PicturePeopleIDs,

                             PictureLocationID = a.PictureLocationID,
                             PictureNation = a.PictureNation,
                             PictureCity = a.PictureCity,
                             PicturePlaceType = a.PicturePlaceType,
                             PicturePlaceName = a.PicturePlaceName,

                             PictureEventID = a.PictureEventID,
                             PictureEventName = a.PictureEventName,
                             PictureEventType = a.PictureEventType,

                             PictureAuthorID = a.PictureAuthorID,
                             PictureAuthorName = a.PictureAuthorName,
                             PictureAuthorLastName = a.PictureAuthorLastName
                         }).ToList();

            ViewPictureAlbumViewModel viewPictureAlbumViewModel = new ViewPictureAlbumViewModel(query, _albumID);

            return View(viewPictureAlbumViewModel);
        }

        [HttpGet]
        public IActionResult AddPictureAlbum(int ID,
                                    string locationID = "",
                                    string eventID = "",
                                    string authorID = "",
                                    string people = "",
                                    string search = ""
                                    )
        {
            var locations = context.Locations.Include(p => p.Place).ToList();
            var events = context.Events.Include(p => p.EventType).ToList();
            var authors = context.Authors.ToList();
            var peopleList = context.PeopleDb.ToList();
            List<int> pictureIDs = new List<int>();

            int albumID = ID;

            if (search == "Location")
            {
                var queryLocation = provider.SelectAllPictures().Where(p => p.PictureLocationID == locationID).ToList();
                

                AddPictureAlbumViewModel searchView = new AddPictureAlbumViewModel(queryLocation, 
                                                                                    locations, 
                                                                                    events, 
                                                                                    authors, 
                                                                                    peopleList,
                                                                                    pictureIDs,
                                                                                    albumID);

                return View(searchView);
            }

            if (search == "Event")
            {
                var queryEvent = provider.SelectAllPictures().Where(p => p.PictureEventID == eventID).ToList();

                AddPictureAlbumViewModel searchView = new AddPictureAlbumViewModel(queryEvent,
                                                                                    locations,
                                                                                    events,
                                                                                    authors,
                                                                                    peopleList,
                                                                                    pictureIDs,
                                                                                    albumID);

                return View(searchView);
            }

            if (search == "Author")
            {
                var queryAuthor = provider.SelectAllPictures().Where(p => p.PictureAuthorID == authorID).ToList();

                AddPictureAlbumViewModel searchView = new AddPictureAlbumViewModel(queryAuthor,
                                                                                    locations,
                                                                                    events,
                                                                                    authors,
                                                                                    peopleList,
                                                                                    pictureIDs,
                                                                                    albumID);

                return View(searchView);
            }

            if (search == "People")
            {
                var queryAllPicture = provider.SelectAllPictures().ToList();

                List<ViewPictureHelper> _queryPeople = new List<ViewPictureHelper>();
                foreach (var i in queryAllPicture)
                {
                    if (i.PicturePeopleIDs is null)
                    {
                        continue;
                    }

                    else
                    {
                        _queryPeople.Add(i);
                    }
                }

                var queryPeople = _queryPeople.Where(p => p.PicturePeopleIDs.Contains(people)).ToList();

                AddPictureAlbumViewModel searchView = new AddPictureAlbumViewModel(queryPeople,
                                                                                    locations,
                                                                                    events,
                                                                                    authors,
                                                                                    peopleList,
                                                                                    pictureIDs,
                                                                                    albumID);

                return View(searchView);
            }

            else
            {
                List<ViewPictureHelper> viewPictureHelpers = new List<ViewPictureHelper>();
                AddPictureAlbumViewModel searchView = new AddPictureAlbumViewModel(viewPictureHelpers,
                                                                                    locations,
                                                                                    events,
                                                                                    authors,
                                                                                    peopleList,
                                                                                    pictureIDs,
                                                                                    albumID);
                return View(searchView);
            }
        }

        [HttpPost]
        public IActionResult AddPictureAlbum(AddPictureAlbumViewModel addPictureAlbumViewModel)
        {
            if (ModelState.IsValid)
            {
                int albumID = addPictureAlbumViewModel.AlbumID;

                List<int> pictureIDs = addPictureAlbumViewModel.PictureIDs;

                foreach (var pictureID in pictureIDs)
                {
                    PictureXAlbum pictureAlbum = new PictureXAlbum()
                    {
                        PictureID = pictureID,
                        AlbumID = albumID
                    };

                    context.PictureXAlbums.Add(pictureAlbum);
                }

                context.SaveChanges();

                return Redirect(string.Format("/Album/AddPIctureAlbum/{0}", albumID));
            }

            return View(addPictureAlbumViewModel);
        }
    }
}
