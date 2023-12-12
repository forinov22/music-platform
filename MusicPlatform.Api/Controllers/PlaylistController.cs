using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Models.DTOs;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Route("api/playlists")]
[ApiController]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistService playlistService;

    public PlaylistController(IPlaylistService playlistService)
    {
        this.playlistService = playlistService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
    {
        var playlists = await playlistService.GetAllPlaylistsAsync();
        return playlists.ToList();
    }

    [HttpGet("{playlistId}")]
    public async Task<ActionResult<PlaylistDto>> GetPlaylist([FromRoute] int playlistId)
    {
        var playlist = await playlistService.GetPlaylistByIdAsync(playlistId);
        return playlist == null ? NotFound() : playlist;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<AlbumDto>> CreatePlaylist([FromBody] PlaylistAdd dto)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();
            
        var playlist = await playlistService.CreatePlaylistAsync(userId, dto);
        return CreatedAtAction(nameof(GetPlaylist), new {playlistId = playlist.Id}, playlist);
    }

    [Authorize(Roles = "User")]
    [HttpDelete("{playlistId}")]
    public async Task<IActionResult> DeletePlaylist(int playlistId)
    {
        var deleted = await playlistService.DeletePlaylistAsync(playlistId);
        return deleted ? NoContent() : NotFound();
    }

    [Authorize(Roles = "User")]
    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetUserPlaylists()
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var playlists = await playlistService.GetUserPlaylistsAsync(userId);
        return playlists.ToList();
    }

    [Authorize(Roles = "User")]
    [HttpGet("liked-playlists")]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetUserLikedPlaylists()
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var playlists = await playlistService.GetUserLikedPlaylists(userId);
        return playlists.ToList();
    }

    [Authorize(Roles = "User")]
    [HttpPost("liked-playlists/{playlistId}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> AddLikedAlbumToUser([FromRoute] int playlistId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var liked = await playlistService.AddLikedPlaylistToUser(userId, playlistId);
        return liked ? NoContent() : NotFound();
    }

    [Authorize(Roles = "User")]
    [HttpDelete("liked-playlists/{playlistId}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> DeleteUserLikedAlbumd([FromRoute] int playlistId)
    {
        if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            return BadRequest();

        var deleted = await playlistService.DeleteUserLikedPlaylist(userId, playlistId);
        return deleted ? NoContent() : NotFound();
    }
}

