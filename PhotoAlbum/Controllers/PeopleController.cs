using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoAlbum.Controllers
{
    public class PeopleController : Controller
    {
        private ApplicationDbContext context;

        public PeopleController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }
           
        [HttpGet]
        public IActionResult Index()
        {
            List<People> people = context.PeopleDb.ToList();
            return View(people);
        }

        public List<string> GenderList()
        {
            List<string> genderList = new List<string>();
            genderList.Add("Male");
            genderList.Add("Female");

            return genderList;
        }

        public List<string> RelationList()
        {
            List<string> relationList = new List<string>();
            relationList.Add("Family");
            relationList.Add("Friend");
            relationList.Add("Pet");

            return relationList;
        }


        [HttpGet]
        public IActionResult Add(List<string> genderList, List<string> relationList )
        {
            genderList = GenderList();
            relationList = RelationList();
            ViewBag.GenderList = genderList;
            ViewBag.RelationList = relationList;

            AddPeopleViewModel addPeopleViewModel = new AddPeopleViewModel();

            return View(addPeopleViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddPeopleViewModel addPeopleViewModel, string gender, string relation)
        {
            if (ModelState.IsValid)
            {
                People people = new People()
                {
                    Name = addPeopleViewModel.Name,
                    LastName = addPeopleViewModel.LastName,
                    Gender = gender,
                    Birthday = addPeopleViewModel.Birthday,
                    Relation = relation
                };

                context.PeopleDb.Add(people);
                context.SaveChanges();

                return Redirect("/People");
            };

            return View(addPeopleViewModel);
        }
    }
}
