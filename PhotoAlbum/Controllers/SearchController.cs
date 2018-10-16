using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Helpers;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.ViewModels;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

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
        public async Task<IActionResult> Index(string locationID="",
                                    string eventID="",
                                    string authorID="",
                                    string people="",
                                    string search=""
                                    )
        {
            var locations = context.Locations.Include(p => p.Place).ToList();
            var events = context.Events.Include(p => p.EventType).ToList();
            var authors = context.Authors.ToList();
            var peopleList = context.PeopleDb.ToList();

            if (search == "search")
            {
                Dictionary<string, string> dictSearchingParameters = new Dictionary<string, string>()
            {
                {"p.PictureLocationID", locationID},
                {"p.PictureEventID", eventID },
                {"p.PictureAuthorID", authorID },
            };

                List<string> listSearchingParameters = new List<string>();
                foreach (KeyValuePair<string, string> item in dictSearchingParameters)
                {
                    if (item.Value == "0")
                    {
                        continue;
                    }

                    if (item.Value == "null")
                    {
                        listSearchingParameters.Add(item.Key + "== null");
                    }

                    else
                    {
                        listSearchingParameters.Add(item.Key + "==" + item.Value + ".ToString()");
                    }
                }

                if (people == "")
                {
                    StringSearchingParametersHelper stringSearchingParameters = new StringSearchingParametersHelper();

                    var query = provider.SelectAllPictures()
                                        .Where(await stringSearchingParameters
                                                        .StringSearchingParamters(listSearchingParameters))
                                        .ToList();

                    SearchViewModel searchView = new SearchViewModel(query, locations, events, authors, peopleList);

                    return View(searchView);
                }

                else // people != ""
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

                    if (locationID != "0" || eventID != "0" || authorID != "0")
                    {
                        StringSearchingParametersHelper stringSearchingParameters = new StringSearchingParametersHelper();

                        var query = provider.SelectAllPictures()
                                            .Where(await stringSearchingParameters
                                                            .StringSearchingParamters(listSearchingParameters))
                                            .ToList();

                        SearchViewModel searchView = new SearchViewModel(query, locations, events, authors, peopleList);

                        var queryWithPeople = (from p in query
                                               join pp in queryPeople
                                               on p.PictureID equals pp.PictureID
                                               select new ViewPictureHelper
                                               {
                                                   PictureID = p.PictureID,
                                                   PictureName = p.PictureName,
                                                   PictureDate = p.PictureDate,
                                                   PictureFavorite = p.PictureFavorite,
                                                   PictureStackID = p.PictureStackID,
                                                   PictureStackIsClassified = p.PictureStackIsClassified,
                                                   PicturePeopleIDs = pp.PicturePeopleIDs,

                                                   PictureLocationID = p.PictureLocationID,
                                                   PictureNation = p.PictureNation,
                                                   PictureCity = p.PictureCity,
                                                   PicturePlaceType = p.PicturePlaceType,
                                                   PicturePlaceName = p.PicturePlaceName,

                                                   PictureEventID = p.PictureEventID,
                                                   PictureEventName = p.PictureEventName,
                                                   PictureEventType = p.PictureEventType,

                                                   PictureAuthorID = p.PictureAuthorID,
                                                   PictureAuthorName = p.PictureAuthorName,
                                                   PictureAuthorLastName = p.PictureAuthorLastName
                                               }).ToList();

                        SearchViewModel searchViewWithPeople = new SearchViewModel(queryWithPeople, locations, events, authors, peopleList);

                        return View(searchViewWithPeople);
                    }

                    else
                    {
                        var queryWithOnlyPeople = _queryPeople.Where(p => p.PicturePeopleIDs.Contains(people)).ToList();
                        SearchViewModel searchView = new SearchViewModel(queryWithOnlyPeople, locations, events, authors, peopleList);

                        return View(searchView);
                    }
                }
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
