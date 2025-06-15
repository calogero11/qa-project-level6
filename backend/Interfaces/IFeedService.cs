using webapi.Models.Entities;

namespace webapi.Interfaces;

public interface IFeedService
{ 
    Task<Feed> CreateFeedAsync(string content, string? title);
    
    Task<IEnumerable<Feed>> GetAllFeedsAsync();

    Task<Feed?> GetFeedByIdAsync(int id);
    
    Task<bool> UpdateFeedAsync(int id, string content, string? title);

    Task<bool> DeleteFeedAsync(int id);
}