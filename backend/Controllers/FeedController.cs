using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using webapi.Interfaces;
using webapi.Models.Requests;

namespace webapi.Controllers;

[Route("feed")]
public class FeedController: ControllerBase
{
    private readonly IFeedService feedService;
    
    public FeedController(IFeedService feedService)
    {
        this.feedService = feedService;
    }
    
    [Authorize, HttpPost]
    public async Task<IActionResult> Create([FromBody] FeedRequest feedRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await feedService.CreateFeedAsync(feedRequest.Content!, feedRequest.Title);
        
        return CreatedAtAction(nameof(Get), new { id = feedRequest.Content }, null);
    }
    
    [Authorize, HttpGet, Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var feed = await feedService.GetFeedByIdAsync(id);

        if (feed is null)
        {
            return NotFound();
        }
        
        return Ok(feed);
    }
    
    [Authorize, HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await feedService.GetAllFeedsAsync();
        return Ok(posts.ToArray());
    }
    
    [Authorize, HttpPut, Route("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FeedRequest feedRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updated = await feedService.UpdateFeedAsync(id, feedRequest.Content!, feedRequest.Title);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [Authorize, HttpDelete, Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await feedService.DeleteFeedAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}