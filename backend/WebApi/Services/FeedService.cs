using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models.Entities;

namespace WebApi.Services;

public class FeedService: IFeedService
{
    private readonly DatabaseContext databaseContext;
    private readonly IIdentityService identityService;
    
    public FeedService(
        DatabaseContext databaseContext,
        IIdentityService identityService)
    {
        this.databaseContext = databaseContext;
        this.identityService = identityService;
    }
    
    public async Task<Feed> CreateFeedAsync(string content, string? title)
    {
        var feed = new Feed
        {
            Title = title,
            Content = content,
            UserGuid = await identityService.GetUserGuidAsync(),
            UserName = await identityService.GetUserNameAsync(),
            UploadedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Europe/London")),
            LastUpdatedDate = null
        };
            
        databaseContext.Feeds.Add(feed);
        await databaseContext.SaveChangesAsync();

        return feed;
    }

    public async Task<IEnumerable<Feed>> GetAllFeedsAsync()
    {
        return await databaseContext.Feeds.OrderBy(feed => feed.UploadedDate).ToListAsync();
    }

    public async Task<Feed?> GetFeedByIdAsync(int id)
    {
        return await databaseContext.Feeds.FindAsync(id);
    }

    public async Task<bool> TryUpdateFeedAsync(Feed feed, string content, string? title)
    {
        try
        {
            feed.Title = title;
            feed.Content = content;
            feed.LastUpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Europe/London"));
            await databaseContext.SaveChangesAsync();
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> TryDeleteFeedAsync(Feed feed)
    {
        try
        {
            databaseContext.Feeds.Remove(feed);
            await databaseContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}