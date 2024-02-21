namespace EventMangmentSystem.Models
{
    public class UserEvent
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
