namespace EventMangmentSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Id of the user who posted the comment
        public string Text { get; set; } // Content of the comment
        public int EventId { get; set; }
        public User User { get; set; }


        // Optional: Add other properties like DateCreated, etc. if needed
    }
}
