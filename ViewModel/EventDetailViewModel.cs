using EventMangmentSystem.Models;

namespace EventMangmentSystem.ViewModel
{
    public class EventDetailViewModel
    {
        public Event Event { get; set; }
        public Genre Genre { get; set; }
        public User User { get; set; }
    }
}
