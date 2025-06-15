using System.Text.RegularExpressions;
using webapi.Models.Entities;

namespace webapi.Data.Seeders;

public static class FeedSeeder
{
    public static async Task SeedFeedsAsync(DatabaseContext databaseContext)
    {
        var users = databaseContext.Users.ToList();

        foreach (var user in users)
        {
            if (databaseContext.Feeds.ToList().FirstOrDefault(feed => feed.UserGuid != Guid.Parse(user.Id)) is not null)
            {
                continue;
            }

            var accountIdentifier = Regex.Replace(user.Email!, @"\D", "");

            if (string.IsNullOrWhiteSpace(accountIdentifier))
            {
                accountIdentifier = "Admin";
            }
                
            databaseContext.Feeds.Add(
                new Feed { 
                    Title = $"Title {accountIdentifier}",
                    Content = $"Content {accountIdentifier}",
                    UserName = user.UserName,
                    UserGuid = Guid.Parse(user.Id),
                    UploadedDate = DateTime.UtcNow
                });
        }

        await databaseContext.SaveChangesAsync();
    }
}