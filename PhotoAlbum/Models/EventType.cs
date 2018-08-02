using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class EventType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<Event> Events {get; set;}
    }
}
