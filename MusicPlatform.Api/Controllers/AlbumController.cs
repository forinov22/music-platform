using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Models.DTOs;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Route("api/albums")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService albumService;

    public AlbumController(IAlbumService albumService)
    {
        this.albumService = albumService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbums()
    {
        var albums = await albumService.GetAllAlbumsAsync();
        return albums.ToList();
    }

    [HttpGet("{albumId}")]
    public async Task<ActionResult<AlbumDto>> GetAlbum([FromRoute] int albumId)
    {
        var album = await albumService.GetAlbumByIdAsync(albumId);
        return album;
    }

    [Authorize(Roles = "Artist")]
    [HttpPost]
    public async Task<ActionResult<AlbumDto>> CreateAlbum([FromForm] AlbumAdd dto)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int artistId))
            return BadRequest();
            
        var album = await albumService.CreateAlbumAsync(artistId, dto);
        return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album);
    }

    [Authorize(Roles = "Artist")]
    [HttpDelete("{albumId}")]
    public async Task<IActionResult> DeleteAlbum(int albumId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int artistId))
            return BadRequest();

        var deleted = await albumService.DeleteAlbumAsync(albumId, artistId);
        return NoContent();
    }

    [Authorize(Roles = "User")]
    [HttpGet("liked-albums")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetUserLikedAlbums()
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var albums = await albumService.GetUserLikedAlbumsAsync(userId);
        return albums.ToList();
    }

    [Authorize(Roles = "User")]
    [HttpPost("liked-albums/{albumId}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> AddLikedAlbumToUser([FromRoute] int albumdId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var liked = await albumService.AddLikedAlbumToUserAsync(userId, albumdId);
        return liked ? NoContent() : NotFound();
    }

    [Authorize(Roles = "User")]
    [HttpDelete("liked-albums/{albumId}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> DeleteUserLikedAlbumd([FromRoute] int albumdId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var deleted = await albumService.DeleteAlbumAsync(userId, albumdId);
        return deleted ? NoContent() : NotFound();
    }
}


