using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventMangmentSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        public  User Artist { get; set; }
        [Required]
        public string ArtistId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public required string Place { get; set; }
        public required Genre Genre { get; set; }
        [Required]
        public int GenreId { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }

        public List<Comment> Comments { get; set; } 

        public Event()
        {
            Comments = new List<Comment>();
        }



    }
} 
