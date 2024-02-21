using EventMangmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace EventMangmentSystem.ViewModel
{
    public class AddCommentViewModel
    {
        public AddCommentViewModel()
        {
            Comments = new List<Comment>();
        }
        [Required(ErrorMessage = "Please enter a comment.")]
        [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Text { get; set; } // Single comment text
        public int EventId { get; set; }
        public string UserName { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
