using System.Security.Claims;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using webapi.Services;

namespace webapi.UnitTests.Services;

public class IdentityServiceTests
{
    private const string UserGuidString = "01977056-4634-79ad-a49b-3b0aa1d74256";
    private const string UsernameAndEmail = "TestUser@test.com";
    
    private UserManager<IdentityUser> userManager;
    private IdentityService sut;
    
    [SetUp]
    public void Setup()
    {
        var httpContextAccessor = A.Fake<IHttpContextAccessor>();
        userManager = A.Fake<UserManager<IdentityUser>>();
        
        sut = new IdentityService(httpContextAccessor, userManager);
    }
    
    #region GetAuthInfoAsync
    
    [Test]
    public async Task GivenUserTokenHasAllRequiredClaims_WhenGetAuthInfoAsync_ThenReturnAuthInfo()
    {
        // Arrange
        var user = CreateIdentityUser();
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        A.CallTo(() => userManager.GetRolesAsync(user)).Returns([
            "Admin",
            "User"
        ]);
        
        // Act
        var result = await sut.GetAuthInfoAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserGuid, Is.EqualTo(Guid.Parse(UserGuidString)));
            Assert.That(result.Email, Is.EqualTo(UsernameAndEmail));
            Assert.That(result.Roles, Is.Not.Null);
            Assert.That(result.Roles.Count(), Is.EqualTo(2));    
        });
    }
    
    [Test]
    public async Task GivenUserTokenDoesNotHaveUserRoles_WhenGetAuthInfoAsync_ThenReturnEmptyCollectionForRoles()
    {
        // Arrange
        var user = CreateIdentityUser();
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        A.CallTo(() => userManager.GetRolesAsync(user)).Returns([]);
        
        // Act
        var result = await sut.GetAuthInfoAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserGuid, Is.EqualTo(Guid.Parse(UserGuidString)));
            Assert.That(result.Email, Is.EqualTo(UsernameAndEmail));
            Assert.That(result.Roles, Is.Not.Null);
            Assert.That(result.Roles.Count(), Is.EqualTo(0));    
        });
    }
    
    [Test]
    public void GivenUserTokenDoesNotHaveUserGuidClaims_WhenGetAuthInfoAsync_ThenThrowUnauthorizedAccessException()
    {
        // Arrange
        var user = CreateIdentityUser();
        user.Id = null!;
        
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        
        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async() => await sut.GetAuthInfoAsync());
    }
    
    [Test]
    public void GivenUserTokenDoesNotHaveUserEmailClaims_WhenGetAuthInfoAsync_ThenThrowUnauthorizedAccessException()
    {
        // Arrange
        var user = CreateIdentityUser();
        user.Email = null;
        
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        
        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async() => await sut.GetAuthInfoAsync());
    }
    
    #endregion
    
    #region GetUserGuidAsync

    [Test]
    public async Task GivenUserTokenHasUserGuidClaims_WhenGetUserGuidAsync_ThenReturnUserGuid()
    {
        // Arrange
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(CreateIdentityUser());
        
        // Act
        var result = await sut.GetUserGuidAsync();
        
        // Assert
        Assert.That(result, Is.EqualTo(Guid.Parse(UserGuidString)));        
    }
    
    [Test]
    public void GivenUserTokenDoesNotHaveUserGuidClaims_WhenGetUserGuidAsync_ThenThrowUnauthorizedAccessException()
    {
        // Arrange
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((IdentityUser) null!);
        
        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await sut.GetUserGuidAsync());
    }

    #endregion
    
    #region GetUserNameAsync
    
    [Test]
    public async Task GivenUserTokenHasUserNameClaims_WhenGetUserNameAsync_ThenReturnUserName()
    {
        // Arrange
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(CreateIdentityUser());
        
        // Act
        var result = await sut.GetUserNameAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(UsernameAndEmail));
        });
    }
    
    [Test]
    public void GivenUserTokenDoesNotHaveUserNameClaims_WhenGetUserNameAsync_ThenThrowUnauthorizedAccessException()
    {
        // Arrange
        A.CallTo(() => userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((IdentityUser) null!);
        
        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await sut.GetUserNameAsync());
    }
    
    #endregion

    private static IdentityUser CreateIdentityUser()
    {
        return new IdentityUser
        {
            Id = UserGuidString,
            UserName = UsernameAndEmail,
            Email = UsernameAndEmail
        };
    }
    
    [TearDown]
    public void DisposeAsync()
    {
        userManager.Dispose();   
    }
}