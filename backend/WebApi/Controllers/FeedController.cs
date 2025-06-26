using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;
using WebApi.Models.Requests;

namespace WebApi.Controllers;

[Route("feed")]
public class FeedController: ControllerBase
{
    private readonly IFeedService feedService;
    private readonly IIdentityService identityService;
    
    public FeedController(IFeedService feedService, IIdentityService identityService)
    {
        this.feedService = feedService;
        this.identityService = identityService;
    }
    
    /// <summary>
    /// Creates a new feed item.
    /// </summary>
    /// <remarks> Requires authorization </remarks>
    /// <param name="feedRequest">The feed content and title</param>
    /// <returns>Returns 201 Created with location header pointing to the new feed</returns>
    /// <response code="201">Feed successfully created</response>
    /// <response code="400">Invalid request body</response>
    /// <response code="401">Unauthorized access</response>
    [Authorize, HttpPost]
    public async Task<IActionResult> Create([FromBody] FeedRequest feedRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var feed = await feedService.CreateFeedAsync(feedRequest.Content!, feedRequest.Title);
        
        return CreatedAtAction(nameof(Get), new { id = feed.Id }, null);
    }
    
    /// <summary>
    /// Retrieves a feed by its unique ID
    /// </summary>
    /// <remarks> Requires authorization </remarks>
    /// <param name="id">The integer ID of the feed</param>
    /// <returns>Returns the feed data if found</returns>
    /// <response code="200">Feed found and returned successfully</response>
    /// <response code="404">Feed with specified ID was not found</response>
    /// <response code="401">Unauthorized access</response>
    [Authorize, HttpGet, Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var feed = await feedService.GetFeedByIdAsync(id);

        if (feed is null)
        {
            return NotFound();
        }
        
        return Ok(feed);
    }
    
    /// <summary>
    /// Retrieves all feeds
    /// </summary>
    /// <remarks> Requires authorization </remarks>
    /// <returns>Returns an array of all feeds</returns>
    /// <response code="200">Successfully retrieved all feeds</response>
    /// <response code="401">Unauthorized access</response>
    [Authorize, HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await feedService.GetAllFeedsAsync();
        return Ok(posts.ToArray());
    }
    
    /// <summary>
    /// Updates an existing feed by ID.
    /// </summary>
    /// <remarks> Requires authorization </remarks>
    /// <param name="id">The ID of the feed to update</param>
    /// <param name="feedRequest">The updated feed content and title</param>
    /// <returns>No content if update is successful</returns>
    /// <response code="204">Feed updated successfully</response>
    /// <response code="400">Invalid request body</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="403">User does not have permission to update this feed</response>
    /// <response code="404">Feed with specified ID was not found</response>
    [Authorize, HttpPut, Route("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] FeedRequest feedRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var feed = await feedService.GetFeedByIdAsync(id);

        if (feed is null)
        {
            return NotFound();
        }
        
        if (feed.UserGuid != await identityService.GetUserGuidAsync() && 
            !(await identityService.GetUserRolesAsync()).Contains("Admin"))
        {
            return StatusCode(403, "The request is Forbidden");
        }
        
        var updated = await feedService.TryUpdateFeedAsync(feed, feedRequest.Content!, feedRequest.Title);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    /// <summary>
    /// Deletes a feed by its ID
    /// </summary>
    /// <remarks> Requires authorization </remarks>
    /// <param name="id">The ID of the feed to delete</param>
    /// <returns>No content if deletion is successful</returns>
    /// <response code="204">Feed deleted successfully</response>
    /// <response code="400">Failed to delete the feed</response>
    /// <response code="401">Unauthorized access</response>
    /// <response code="403">User does not have permission to delete this feed</response>
    /// <response code="404">Feed with specified ID was not found</response>
    [Authorize, HttpDelete, Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var feed = await feedService.GetFeedByIdAsync(id);

        if (feed is null)
        {
            return NotFound();
        }
            
        if (feed.UserGuid != await identityService.GetUserGuidAsync() &&
            !(await identityService.GetUserRolesAsync()).Contains("Admin"))
        {
            return StatusCode(403, "The request is Forbidden");
        }
        
        return await feedService.TryDeleteFeedAsync(feed) ?
            NoContent() :
            BadRequest();
    }
}
