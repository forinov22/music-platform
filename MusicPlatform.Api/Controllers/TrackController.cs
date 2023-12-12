using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Models.DTOs;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Route("api/tracks")]
[ApiController]
public class TrackController : ControllerBase
{
    private readonly ITrackService trackService;

    public TrackController(ITrackService trackService)
    {
        this.trackService = trackService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrackDto>>> GetAllTracks()
    {
        var tracks = await trackService.GetAllTracksAsync();
        return tracks.ToList();
    }

    [HttpGet("{trackId}")]
    public async Task<ActionResult<TrackDto>> GetTrack([FromRoute] int trackId)
    {
        var track = await trackService.GetTrackByIdAsync(trackId);
        return track == null ? NotFound() : track;
    }

    [Authorize(Roles = "Artist")]
    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromForm] TrackAdd dto)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int artistId))
            return BadRequest();

        var track = await trackService.CreateTrackAsync(artistId, dto);
        return CreatedAtAction(nameof(GetTrack), new { trackId = track.Id }, track);
    }

    [Authorize(Roles = "Artist")]
    [HttpDelete("{trackId}")]
    public async Task<IActionResult> DeleteTrack([FromRoute] int trackId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int artistId))
            return BadRequest();
        
        var deleted = await trackService.DeleteTrackAsync(trackId, artistId);
        return deleted ? NoContent() : NotFound();
    }

    [Authorize(Roles = "User")]
    [HttpGet("liked-tracks")]
    public async Task<ActionResult<IEnumerable<TrackDto>>> GetUserLikedTracks()
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var tracks = await trackService.GetUserLikedTracksAsync(userId);
        return tracks.ToList();
    }

    [Authorize(Roles = "User")]
    [HttpPost("liked-tracks/{trackId}")]
    public async Task<ActionResult> AddLikeTrackToUser([FromRoute] int trackId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var liked = await trackService.AddLikedTrackToUserAsync(userId, trackId);
        return liked ? NoContent() : NotFound();
    }

    [Authorize(Roles = "User")]
    [HttpDelete("liked-tracks/{trackId}")]
    public async Task<ActionResult> DeleteUserLikedTrack([FromRoute] int trackId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var deleted = await trackService.DeleteUserLikedTrackAsync(userId, trackId);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("track-played/{trackId}")]
    public async Task<ActionResult<HistoryDto>> TrackPlayed([FromRoute] int trackId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var history = await trackService.UpdateUserHistoryAsync(userId, trackId);
        return history;
    }
}
