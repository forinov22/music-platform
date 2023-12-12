using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Models.DTOs;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Route("api/artists")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IArtistService artistService;    

    public ArtistController(IArtistService artistService)
    {
        this.artistService = artistService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistDto>>> GetAllArtists()
    {
        var artists = await artistService.GetAllArtistsAsync();
        return artists.ToList();
    }

    [HttpGet("{artistId}")]
    public async Task<ActionResult<ArtistDto>> GetTrack([FromRoute] int artistId)
    {
        var artist = await artistService.GetArtistByIdAsync(artistId);
        return artist == null ? NotFound() : artist;
    }

    [HttpGet("{artistId}/tracks")]
    public async Task<ActionResult<IEnumerable<TrackDto>>> GetAllArtistTracks([FromRoute] int artistId)
    {
        var tracks = await artistService.GetAllArtistTracks(artistId);
        return tracks == null ? NotFound() : tracks.ToList();
    }


    [HttpGet("{artistId}/albums")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAllArtistAlbums([FromRoute] int artistId)
    {
        var albums = await artistService.GetAllArtistAlbums(artistId);
        return albums == null ? NotFound() : albums.ToList();
    }

    [Authorize(Roles = "User")]
    [HttpGet("followed-artists")]
    public async Task<ActionResult<IEnumerable<ArtistDto>>> GetUserFollowedArtists()
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var followedArtists = await artistService.GetUserFollowedArtists(userId);
        return followedArtists.ToList();
    }

    [Authorize(Roles = "User")]
    [HttpPost("followed-artists/{artistId}")]
    public async Task<IActionResult> AddFollowedArtist(int artistId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var followed = await artistService.AddFollowedArtistToUser(artistId, userId);
        return followed ? NoContent() : NotFound();
    }

    [Authorize(Roles = "User")]
    [HttpDelete("followed-artists/{artistId}")]
    public async Task<IActionResult> DeleteFollowedArtist(int artistId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var deleted = await artistService.DeleteUserFollowedArtist(artistId, userId);
        return deleted ? NoContent() : NotFound();
    }
}


