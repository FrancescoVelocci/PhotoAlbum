using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

// AUTHOR Controller

namespace PhotoAlbum.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext context;

        public AuthorController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Author> authors = context.Authors.ToList();

            return View(authors);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddAuthorViewModel addAuthorViewModel = new AddAuthorViewModel();
            
            return View(addAuthorViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddAuthorViewModel addAuthorViewModel)
        {
            if (ModelState.IsValid)
            {
                Author author = new Author()
                {
                    Name = addAuthorViewModel.Name,
                    LastName = addAuthorViewModel.LastName,
                    Professional = addAuthorViewModel.Professional
                };

                context.Authors.Add(author);
                context.SaveChanges();

                return Redirect("/Author");
            }

            return View(addAuthorViewModel);
        }
    }
}
