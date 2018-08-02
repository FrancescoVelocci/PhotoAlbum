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
        public IActionResult View(int ID)
        {
            List<Picture> pictures = context.Pictures.Where(p => p.StackID == ID).ToList();
            return View(pictures);
        }
    }
}
