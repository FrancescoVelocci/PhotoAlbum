using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;

// EVENT TYPE Controller

namespace PhotoAlbum.Controllers
{
    public class EventTypeController : Controller
    {
        private readonly ApplicationDbContext context;

        public EventTypeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<EventType> eventType = context.EventTypes.ToList();
            return View(eventType);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddEventTypeViewModel addEventTypeViewModel = new AddEventTypeViewModel();

            return View(addEventTypeViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddEventTypeViewModel addEventTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                EventType eventType = new EventType()
                {
                    Name = addEventTypeViewModel.Name
                };

                context.EventTypes.Add(eventType);
                context.SaveChanges();

                return View(addEventTypeViewModel);
            }

            return View();
        }
    }
}
