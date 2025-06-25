using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Requests;

public class FeedRequest
{
    public string? Title { get; set; }
    
    [Required]
    public string? Content { get; set; }
}