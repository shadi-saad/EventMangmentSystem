using EventMangmentSystem.Models;

namespace EventMangmentSystem.ViewModel
{
    public class EventViewModel
    {

        public List<Event> Events { get; set; }
        public string CurrentUserId { get; set; }
        public List<Genre> Genres { get; set; }
        public int? SelectedGenreId { get; set; }
        public int PageSize { get; set; }
        public int TotalEvents { get; set; }
        public int CurrentPage { get; set; }


    }
}
