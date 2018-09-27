using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Helpers;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.ViewModels;

// SEARCH Controller

namespace PhotoAlbum.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext context;
        private DataProvider provider;

        public SearchController(ApplicationDbContext dbContext)
        {
            provider = new DataProvider(dbContext);
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index(string locationID = "",
                                    string eventID = "",
                                    string authorID = "",
                                    string people = "",
                                    string search = "")
        {
            var locations = context.Locations.Include(p => p.Place).ToList();
            var events = context.Events.Include(p => p.EventType).ToList();
            var authors = context.Authors.ToList();
            var peopleList = context.PeopleDb.ToList();

            if (search == "Location")
            {
                var queryLocation = provider.SelectAllPictures().Where(p => p.PictureLocationID == locationID).ToList();

                SearchViewModel searchView = new SearchViewModel(queryLocation, locations, events, authors, peopleList);

                return View(searchView);
            }

            if (search == "Event")
            {
                var queryEvent = provider.SelectAllPictures().Where(p => p.PictureEventID == eventID).ToList();

                SearchViewModel searchView = new SearchViewModel(queryEvent, locations, events, authors, peopleList);

                return View(searchView);
            }

            if (search == "Author")
            {
                var queryAuthor = provider.SelectAllPictures().Where(p => p.PictureAuthorID == authorID).ToList();

                SearchViewModel searchView = new SearchViewModel(queryAuthor, locations, events, authors, peopleList);

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
               
                SearchViewModel searchView = new SearchViewModel(queryPeople, locations, events, authors, peopleList);

                return View(searchView);
            }

            else
            {
                List<ViewPictureHelper> viewPictureHelpers = new List<ViewPictureHelper>();
                SearchViewModel searchView = new SearchViewModel(viewPictureHelpers, locations, events, authors, peopleList);
                return View(searchView);
            }
        }
    }
}
