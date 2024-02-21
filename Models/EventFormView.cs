
namespace EventMangmentSystem.Models
{
    public class EventFormView
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }
        public string Place { get; set; }
        public int Genre { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
