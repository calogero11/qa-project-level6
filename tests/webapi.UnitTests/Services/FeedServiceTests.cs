using webapi.Services;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Interfaces;
using webapi.Models.Entities;
using webapi.UnitTests.Helpers;

namespace webapi.UnitTests.Services;

public class FeedServiceTests
{
    private const string UserGuidString = "01977056-4634-79ad-a49b-3b0aa1d74256";
    private const string UserGuidString2 = "01977058-eddc-714f-91a5-6ff0c29eccc9";
    
    private IIdentityService identityService;
    private DatabaseContext databaseContext;
    private FeedService sut;
    
    [SetUp]
    public void Setup()
    {
        databaseContext = TestDbContextFactory.Create("TestDatabase");
        identityService = A.Fake<IIdentityService>();
            
        sut = new FeedService(databaseContext, identityService);
    }

    #region CreateFeedAsync

    [Test]
    public async Task GivenValidForm_WhenCreateFeedAsync_ThenFeedIsCreated()
    {
        // Arrange
        A.CallTo(() => identityService.GetUserGuidAsync()).Returns(Guid.Parse(UserGuidString));
        A.CallTo(() => identityService.GetUserNameAsync()).Returns("TestUser1@test.com");
        
        // Act
        var result = await sut.CreateFeedAsync("TestContent", "TestTitle");
        
        // Assert
        Assert.Multiple(() =>
        { 
            Assert.That(result.Title, Is.EqualTo("TestTitle"));
            Assert.That(result.Content, Is.EqualTo("TestContent"));
            Assert.That(result.UserGuid, Is.EqualTo(Guid.Parse(UserGuidString)));
            Assert.That(result.UserName, Is.EqualTo("TestUser1@test.com"));
            Assert.That(result.LastUpdatedDate, Is.Null);
        });
        
        Assert.Multiple(() =>
        {
            var feedCreated = databaseContext.Feeds.First();
            Assert.That(feedCreated.Title, Is.EqualTo("TestTitle"));
            Assert.That(feedCreated.Content, Is.EqualTo("TestContent"));
            Assert.That(feedCreated.UserGuid, Is.EqualTo(Guid.Parse(UserGuidString)));
            Assert.That(feedCreated.UserName, Is.EqualTo("TestUser1@test.com"));
            Assert.That(feedCreated.LastUpdatedDate, Is.Null);
        });
    }
    
    [Test]
    public void GivenInValidForm_WhenCreateFeedAsync_ThenExceptionIsThrown()
    {
        Assert.ThrowsAsync<DbUpdateException>(async () =>
            await sut.CreateFeedAsync(null!, "TestTitle"));
    }

    #endregion
    
    #region GetAllFeedsAsync

    [Test]
    public async Task GivenDBHasFeed_WhenGetAllFeedsAsync_ThenReturnFeed()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.GetAllFeedsAsync();
        
        // Assert
        result = result.ToList();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
    }
    
    [Test]
    public async Task GivenDBDoesNotHaveAnyFeeds_GetAllFeedsAsync_ThenReturnEmptyList()
    {
        // Act
        var result = await sut.GetAllFeedsAsync();
        
        // Assert
        result = result.ToList();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(0));
    }
    
    #endregion
    
    #region DeleteFeedAsync
    
    [Test]
    public async Task GivenDBHasSelectFeed_WhenDeleteFeedAsync_ThenRemoveFeed()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.DeleteFeedAsync(1);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(databaseContext.Feeds.FirstOrDefault(feed => feed.Id == 1), Is.Null);
            Assert.That(databaseContext.Feeds.Count(), Is.EqualTo(2));
        });
    }
    
    [Test]
    public async Task GivenDBDoesNotHaveSelectFeed_WhenDeleteFeedAsync_ThenDoNotRemoveFeed()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.DeleteFeedAsync(4);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(databaseContext.Feeds.FirstOrDefault(feed => feed.Id == 4), Is.Null);
            Assert.That(databaseContext.Feeds.Count(), Is.EqualTo(3));
        });
    }

    #endregion
    
    #region UpdateFeedAsync
    
    [Test]
    public async Task GivenDBHasSelectFeed_WhenUpdateFeedAsync_ThenUpdateFeed()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.UpdateFeedAsync(1, "UpdatedContent", "UpdatedTitle");
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);

            var updatedFeed = databaseContext.Feeds.Single(feed => feed.Id == 1);
            Assert.That(updatedFeed, Is.Not.Null);
            Assert.That(updatedFeed.Title, Is.EqualTo("UpdatedTitle"));
            Assert.That(updatedFeed.Content, Is.EqualTo("UpdatedContent"));
            Assert.That(updatedFeed.LastUpdatedDate, Is.Not.Null);
        });
    }
    
    [Test]
    public async Task GivenDBDoesNotHaveSelectFeed_WhenUpdateFeedAsync_ThenDoNotUpdateFeed()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.UpdateFeedAsync(4, "UpdatedContent", "UpdatedTitle");
        
        // Assert
        Assert.Multiple( () =>
        {
            Assert.That(result, Is.False);
            
            Assert.That(databaseContext.Feeds.FirstOrDefault(feed => feed.Id == 4), Is.Null);
            Assert.That(databaseContext.Feeds.Count(), Is.EqualTo(3));
        });
    }
    
    #endregion

    #region GetFeedByIdAsync

    [Test]
    public async Task GivenDBHasSelectFeed_WhenGetFeedByIdAsync_ThenReturnFeed()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.GetFeedByIdAsync(1);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(databaseContext.Feeds.Single(feed => feed.Id == 1)));
        });
    }
    
    [Test]
    public async Task GivenDBDoesNotHaveSelectFeed_WhenGetFeedByIdAsync_ThenReturnNull()
    {
        // Arrange
        SeedFakeFeedData();
        
        // Act
        var result = await sut.GetFeedByIdAsync(4);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Null);
            Assert.That(databaseContext.Feeds.FirstOrDefault(feed => feed.Id == 4), Is.Null);
        });
    }
    
    #endregion
    
    private void SeedFakeFeedData()
    {
        databaseContext.Feeds.AddRange(
            new Feed
            {
                Id = 1,
                Title = "Title1",
                Content = "Content1",
                UserGuid = Guid.Parse(UserGuidString),
                UserName = "TestUser1@test.com",
                UploadedDate = new DateTime(2025, 1, 1),
                LastUpdatedDate = new DateTime(2025, 1, 2),
            },
            new Feed
            {
                Id = 2,
                Title = "Title2",
                Content = "Content2",
                UserGuid = Guid.Parse(UserGuidString),
                UserName = "TestUser1@test.com",
                UploadedDate = new DateTime(2025, 1, 3),
                LastUpdatedDate = new DateTime(2025, 1, 4),
            },
            new Feed
            {
                Id = 3,
                Title = "Title3",
                Content = "Content3",
                UserGuid = Guid.Parse(UserGuidString2),
                UserName = "TestUser2@test.com",
                UploadedDate = new DateTime(2025, 1, 5),
                LastUpdatedDate = new DateTime(2025, 1, 6),
            }
        );

        databaseContext.SaveChanges();
    }
    
    [TearDown]
    public async Task DisposeAsync()
    {
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.DisposeAsync();   
    }
}