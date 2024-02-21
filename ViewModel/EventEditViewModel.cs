using EventMangmentSystem.Models;

namespace EventMangmentSystem.ViewModel
{
    public class EventEditViewModel
    {
        public Event Event { get; set; }
        public int Genre { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
