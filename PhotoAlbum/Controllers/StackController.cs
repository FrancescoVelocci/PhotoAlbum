using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

// STACK Controller

namespace PhotoAlbum.Controllers
{
    public class StackController : Controller
    {
        private ApplicationDbContext context;

        public StackController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Stack> stacks = context.Stacks.Where(i=>i.Classified == false).ToList();
            return View(stacks);
        }

        [HttpGet]
        public IActionResult ViewStack(string ID)
        {
            if (ID == null)
            {
                return Redirect("/Picture/Stack");
            }

            else
            {
                return Redirect(string.Format("/Picture/Stack?ID={0}", ID.ToString()));
            }
        }
    }
}
