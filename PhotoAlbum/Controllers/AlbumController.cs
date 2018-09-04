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
        private readonly ApplicationDbContext context;

        public AlbumController(ApplicationDbContext dbContext)
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

                                                       PictureLocationID = pl.LocationID.ToString(),
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

                                                                 PictureLocationID = pl.PictureLocationID,
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
                                                    PictureEventID = pe.EventID.ToString(),
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

                                                     PictureLocationID = pl.PictureLocationID,
                                                     PictureNation = pl.PictureNation,
                                                     PictureCity = pl.PictureCity,
                                                     PicturePlaceType = pl.PicturePlaceType,
                                                     PicturePlaceName = pl.PicturePlaceName,

                                                     PictureEventID = pe.PictureEventID,
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
                                                     PictureAuthorID = pa.AuthorID.ToString(),
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

                                      PictureLocationID = ple.PictureLocationID,
                                      PictureNation = ple.PictureNation,
                                      PictureCity = ple.PictureCity,
                                      PicturePlaceType = ple.PicturePlaceType,
                                      PicturePlaceName = ple.PicturePlaceName,

                                      PictureEventID = ple.PictureEventID,
                                      PictureEventName = ple.PictureEventName,
                                      PictureEventType = ple.PictureEventType,

                                      PictureAuthorID = pa.PictureAuthorID,
                                      PictureAuthorName = pa.PictureAuthorName,
                                      PictureAuthorLastName = pa.PictureAuthorLastName
                                  }).ToList();

            return querySelectAll;
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

            var query = (from a in SelectAllPictures()
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
                var queryLocation = SelectAllPictures().Where(p => p.PictureLocationID == locationID).ToList();
                

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
                var queryEvent = SelectAllPictures().Where(p => p.PictureEventID == eventID).ToList();

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
                var queryAuthor = SelectAllPictures().Where(p => p.PictureAuthorID == authorID).ToList();

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
                var queryAllPicture = SelectAllPictures().ToList();

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
