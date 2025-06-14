using System.ComponentModel.DataAnnotations;

namespace webapi.Models.Requests;

public class FeedRequest
{
    public string? Title { get; set; }
    
    [Required]
    public string Content { get; set; }
}