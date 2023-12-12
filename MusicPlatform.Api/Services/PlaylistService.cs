using Mapster;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Domains;
using MusicPlatform.Api.Exceptions;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface IPlaylistService
{
    Task<List<PlaylistDto>> GetAllPlaylistsAsync();
    Task<List<PlaylistDto>> GetUserPlaylistsAsync(int userId);
    Task<PlaylistDto?> GetPlaylistByIdAsync(int playlistId);
    Task<PlaylistDto> CreatePlaylistAsync(int userId, PlaylistAdd dto);
    Task<bool> DeletePlaylistAsync(int playlistId);

    Task<IEnumerable<PlaylistDto>> GetUserLikedPlaylists(int userId);
    Task<bool> AddLikedPlaylistToUser(int userId, int playlistId);
    Task<bool> DeleteUserLikedPlaylist(int userId, int playlistId);
}


public class PlaylistService : IPlaylistService
{
    private readonly MusicPlatformContext _context;
    private readonly ITrackService _trackService;
    private readonly IWebHostEnvironment _env;

    public PlaylistService(MusicPlatformContext context, IWebHostEnvironment env, ITrackService trackService)
    {
        _context = context;
        _env = env;
        _trackService = trackService;
    }

    public async Task<List<PlaylistDto>> GetAllPlaylistsAsync()
    {
        return await _context.Playlists.ProjectToType<PlaylistDto>()
            .ToListAsync();
    }

    public async Task<List<PlaylistDto>> GetUserPlaylistsAsync(int userId)
    {
        return await _context.Playlists.ProjectToType<PlaylistDto>()
            .Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<PlaylistDto?> GetPlaylistByIdAsync(int playlistId)
    {
        return await _context.Playlists.ProjectToType<PlaylistDto>()
            .FirstOrDefaultAsync(p => p.Id == playlistId);
    }

    public async Task<PlaylistDto> CreatePlaylistAsync(int userId, PlaylistAdd dto)
    {
        var playlist = dto.Adapt<Playlist>();

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new NotFoundException(ExceptionMessages.UserNotFound);

        playlist.UserId = userId;

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();
        
        foreach (var playlistTrackAdd in dto.Tracks) {
            var track = await _context.Tracks.FindAsync(playlistTrackAdd.TrackId);
            if (track == null)
                throw new NotFoundException(ExceptionMessages.TrackNotFound);

            var genre = await _context.Genres.FindAsync(track.GenreId);
            if (genre == null)
                throw new NotFoundException(ExceptionMessages.GenreNotFound);

            var singer = await _context.Artists.FindAsync(track.ArtistId);
            if (singer == null)
                throw new NotFoundException(ExceptionMessages.ArtistNotFound);

            if (!playlist.Genres.Contains(genre))
                playlist.Genres.Add(genre);
            
            if (!playlist.Singers.Contains(singer))
                playlist.Singers.Add(singer);
                
            var playlistTrack = new PlaylistTrack { PlaylistId = playlist.Id, TrackId = track.Id, Order = playlistTrackAdd.Order };
            _context.PlaylistTracks.Add(playlistTrack);
        }

        await _context.SaveChangesAsync();
        return playlist.Adapt<PlaylistDto>();
    }

    public async Task<bool> DeletePlaylistAsync(int playlistId)
    {
        var playlist = await _context.Playlists.FindAsync(playlistId);

        if (playlist == null) return false;

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PlaylistDto>> GetUserLikedPlaylists(int userId)
    {
        return await _context.FavoritePlaylists.ProjectToType<FavoritePlaylist>()
            .Where(fp => fp.UserId == userId)
            .Select(fp => fp.Playlist.Adapt<PlaylistDto>())
            .ToListAsync();
    }

    public async Task<bool> AddLikedPlaylistToUser(int userId, int playlistId)
    {
        var user = await _context.Users.FindAsync(userId);
        var playlist = await _context.Playlists.FindAsync(playlistId);

        if (user == null)
            throw new NotFoundException(ExceptionMessages.UserNotFound);

        if (playlist == null)
            throw new NotFoundException(ExceptionMessages.PlaylistNotFound);

        var userLikedPlaylist = new FavoritePlaylist { UserId = userId, PlaylistId = playlistId, Date = DateOnly.FromDateTime(DateTime.UtcNow) };

        await _context.FavoritePlaylists.AddAsync(userLikedPlaylist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserLikedPlaylist(int userId, int playlistId)
    {
        var userLikedPlaylist = await _context.FavoritePlaylists
            .FirstOrDefaultAsync(ufp => ufp.PlaylistId ==playlistId && ufp.UserId == userId);

        if (userLikedPlaylist != null)
        {
            _context.FavoritePlaylists.Remove(userLikedPlaylist);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
