using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using PhotoAlbum.ViewModels;


// EVENT Controller

namespace PhotoAlbum.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext context;

        public EventController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Event> events = context.Events.ToList();

            return View(events);
        }

        [HttpGet]
        public IActionResult Add()
        {
            IList<EventType> eventTypes = context.EventTypes.ToList();
            AddEventViewModel addEventViewModel = new AddEventViewModel(eventTypes);

            return View(addEventViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddEventViewModel addEventViewModel)
        {
            if (ModelState.IsValid)
            {
                EventType newEventType = context.EventTypes.Single(i => i.ID == addEventViewModel.EventTypeID);

                Event newEvent = new Event
                {
                    Name = addEventViewModel.Name,
                    Description = addEventViewModel.Name,
                    EventType = newEventType,
                };

                context.Events.Add(newEvent);
                context.SaveChanges();

                return Redirect("/Event");
            }

            return View(addEventViewModel);
        }
    }
}
