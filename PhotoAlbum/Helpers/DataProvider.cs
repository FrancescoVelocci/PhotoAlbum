using PhotoAlbum.Data;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Helpers
{
    public class DataProvider
    {
        private ApplicationDbContext context;

        public DataProvider(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IQueryable<Picture> GetPictures()
        {
            return context.Pictures;
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
                if (viewPicturePeopleObj == null)
                {
                    continue;
                }

                else
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
    }
}

/*private DataProvider provider;

        public PictureController(ApplicationDbContext dbContext)
        {
            this.provider = new DataProvider(dbContext);
        }*/
