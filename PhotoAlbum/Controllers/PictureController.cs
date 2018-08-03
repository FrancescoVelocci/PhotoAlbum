using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoAlbum.Controllers
{
    public class PictureController : Controller
    {
        private ApplicationDbContext context;

        public PictureController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            ViewBag.DateSortParmDesc = sortOrder == "Date" ? "date_desc" : "DateDesc";
            ViewBag.DateSortParmAsc = sortOrder == "Date" ? "date_asc" : "DateAsc";
            var pictures = from pic in context.Pictures
                          select pic;
            
            //List<Picture> pictures = context.Pictures.ToList();

            switch(sortOrder)
            {
                case "DateDesc":
                    pictures = pictures.OrderByDescending(pic => pic.Date);
                    break;

                case "DateAsc":
                    pictures = pictures.OrderBy(pic => pic.Date);
                    break;
            }

            return View(pictures.ToList());
        }
    }
}
