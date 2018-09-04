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
                var eventTipes = context.EventTypes.Select(p => p.Name).ToList();

                if (eventTipes.Contains(addEventTypeViewModel.Name))
                {
                    ViewBag.Validation = "Type already exist";
                    return View(addEventTypeViewModel);
                }
                else
                {
                    EventType eventType = new EventType()
                    {
                        Name = addEventTypeViewModel.Name
                    };

                    context.EventTypes.Add(eventType);
                    context.SaveChanges();

                    return Redirect("/EventType");
                }
            }

            return View(addEventTypeViewModel);
        }
    }
}
