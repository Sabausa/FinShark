using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment;

public class UpdateCommentDto
{
    [Required]
    [Length(5,50, ErrorMessage = "Title mist be between 5 and 50 characters")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [Length(5,1000, ErrorMessage = "Title mist be between 5 and 1000 characters")]
    public string Content { get; set; } = string.Empty;
    
}