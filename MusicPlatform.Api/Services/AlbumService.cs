using Mapster;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Domains;
using MusicPlatform.Api.Exceptions;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface IAlbumService
{
    Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync();
    Task<AlbumDto> GetAlbumByIdAsync(int albumId);
    Task<AlbumDto> CreateAlbumAsync(int artistId, AlbumAdd dto);
    Task<bool> DeleteAlbumAsync(int albumId, int artistId);

    Task<IEnumerable<AlbumDto>> GetUserLikedAlbumsAsync(int userId);
    Task<bool> AddLikedAlbumToUserAsync(int albumId, int userId);
    Task<bool> DeleteUserLikedAlbumAsync(int albumId, int userId);
}


public class AlbumService : IAlbumService
{
    private readonly MusicPlatformContext _context;
    private readonly IWebHostEnvironment environment;
    private readonly ITrackService _trackService;

    public AlbumService(MusicPlatformContext context, IWebHostEnvironment environment, ITrackService trackService)
    {
        _context = context;
        this.environment = environment;
        _trackService = trackService;
    }

    public async Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync()
    {
        return await _context.Albums.ProjectToType<AlbumDto>()
            .ToListAsync();
    }

    public async Task<AlbumDto> GetAlbumByIdAsync(int albumId) {
        var album = await _context.Albums.ProjectToType<AlbumDto>()
            .FirstOrDefaultAsync(a => a.Id == albumId);

        if (album == null)
            throw new NotFoundException(ExceptionMessages.AlbumNotFound);

        return album;
    }

    public async Task<AlbumDto> CreateAlbumAsync(int artistId, AlbumAdd dto)
    {
        var album = dto.Adapt<Album>();

        var coverUrl = Path.Combine("covers", "albums", Guid.NewGuid().ToString() + Path.GetExtension(dto.CoverFile.FileName));
        var coverUploadPath = Path.Combine(environment.WebRootPath, coverUrl);

        await using (var fs = new FileStream(coverUploadPath, FileMode.Create)) {
            await dto.CoverFile.CopyToAsync(fs);
        }

        var artist = await _context.Artists.FindAsync(artistId);
        if (artist == null)
            throw new NotFoundException(ExceptionMessages.ArtistNotFound);

        album.ArtistId = artistId;
        album.CoverUrl = coverUrl;

        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        
        foreach (var albumTrackAdd in dto.Tracks) {
            var track = await _trackService.CreateTrackAsync(artistId, albumTrackAdd.Track);

            var genre = await _context.Genres.FindAsync(track.GenreId);
            if (genre == null)
                throw new NotFoundException(ExceptionMessages.GenreNotFound);

            if (!album.Genres.Contains(genre))
                album.Genres.Add(genre);
                
            var albumTrack = new AlbumTrack { AlbumId = album.Id, TrackId = track.Id, Order = albumTrackAdd.Order };
            _context.AlbumTracks.Add(albumTrack);
        }

        await _context.SaveChangesAsync();
        return album.Adapt<AlbumDto>();
    }

    public async Task<bool> DeleteAlbumAsync(int albumId, int artistId)
    {
        var album = await _context.Albums.FindAsync(albumId);
        
        if (album == null) return false;

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AlbumDto>> GetUserLikedAlbumsAsync(int userId)
    {
        return await _context.FavoriteAlbums.ProjectToType<FavoriteAlbum>()
            .Where(fa => fa.UserId == userId)
            .Select(fa => fa.Album.Adapt<AlbumDto>())
            .ToListAsync();
    }

    public async Task<bool> AddLikedAlbumToUserAsync(int albumId, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var album = await _context.Albums.FindAsync(albumId);

        if (user == null)
            throw new NotFoundException(ExceptionMessages.UserNotFound);

        if (album == null)
            throw new NotFoundException(ExceptionMessages.AlbumNotFound);

        var userLikedAlbum = new FavoriteAlbum { UserId = userId, AlbumId = albumId, Date = DateOnly.FromDateTime(DateTime.UtcNow) };
        await _context.FavoriteAlbums.AddAsync(userLikedAlbum);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserLikedAlbumAsync(int albumId, int userId)
    {
        var userLikedAlbum = await _context.FavoriteAlbums
            .FirstOrDefaultAsync(fa => fa.AlbumId == albumId && fa.UserId == userId);

        if (userLikedAlbum != null) {
            _context.FavoriteAlbums.Remove(userLikedAlbum);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}

