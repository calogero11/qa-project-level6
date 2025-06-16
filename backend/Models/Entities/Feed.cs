using System.ComponentModel.DataAnnotations;

namespace webapi.Models.Entities;

public class Feed
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid UserGuid { get; set; }
    
    [Required, MaxLength(255)]
    public string? UserName { get; set; }
    
    [MaxLength(255)]
    public string? Title { get; set; }

    [Required, MaxLength(255)]
    public string? Content { get; set; }

    [Required]
    public DateTime UploadedDate { get; set; }
    
    public DateTime? LastUpdatedDate { get; set; }
}