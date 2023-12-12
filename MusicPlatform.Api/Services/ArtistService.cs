using Mapster;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Domains;
using MusicPlatform.Api.Exceptions;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface IArtistService
{
    Task<IEnumerable<ArtistDto>> GetAllArtistsAsync();
    Task<ArtistDto> GetArtistByIdAsync(int artistId);
    Task<ArtistDto> CreateArtistAsync(ArtistAdd dto);
    Task<IEnumerable<TrackDto>> GetAllArtistTracks(int artistId);
    Task<IEnumerable<AlbumDto>> GetAllArtistAlbums(int artistId);

    Task<IEnumerable<ArtistDto>> GetUserFollowedArtists(int userId);
    Task<bool> AddFollowedArtistToUser(int artistId, int userId);
    Task<bool> DeleteUserFollowedArtist(int artistId, int userId);
}


public class ArtistService : IArtistService
{
    private readonly MusicPlatformContext context;

    public ArtistService(MusicPlatformContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<ArtistDto>> GetAllArtistsAsync()
    {
        return await context.Artists.ProjectToType<ArtistDto>().ToListAsync();
    }

    public async Task<ArtistDto> GetArtistByIdAsync(int artistId)
    {
        var artist = await context.Artists.ProjectToType<ArtistDto>()
            .FirstOrDefaultAsync(a => a.Id == artistId);

        if (artist == null)
            throw new NotFoundException(ExceptionMessages.ArtistNotFound);

        return artist;
    }

    public async Task<ArtistDto> CreateArtistAsync(ArtistAdd dto)
    {
        var artist = dto.Adapt<Artist>();
        artist.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        await context.AddAsync(artist);
        await context.SaveChangesAsync();
        return artist.Adapt<ArtistDto>();
    }

    public async Task<IEnumerable<TrackDto>> GetAllArtistTracks(int artistId)
    {
        return await context.Tracks.ProjectToType<TrackDto>()
            .Where(track => track.ArtistId == artistId)
            .ToListAsync();
    }

    public async Task<IEnumerable<AlbumDto>> GetAllArtistAlbums(int artistId)
    {
        return await context.Albums.ProjectToType<AlbumDto>()
            .Where(album => album.ArtistId == artistId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ArtistDto>> GetUserFollowedArtists(int userId)
    {
        return await context.FavoriteArtists.ProjectToType<FavoriteArtist>()
            .Where(fa => fa.UserId == userId)
            .Select(fa => fa.Artist.Adapt<ArtistDto>())
            .ToListAsync();
    }

    public async Task<bool> AddFollowedArtistToUser(int artistId, int userId)
    {
        var userFollowedArtist = new FavoriteArtist { UserId = userId, ArtistId = artistId, Date = DateOnly.FromDateTime(DateTime.UtcNow) };

        context.FavoriteArtists.Add(userFollowedArtist);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserFollowedArtist(int artistId, int userId)
    {
        var userFollowedArtist = await context.FavoriteArtists
            .FirstOrDefaultAsync(ufa => ufa.ArtistId == artistId && ufa.UserId == userId);

        if (userFollowedArtist != null)
        {
            context.FavoriteArtists.Remove(userFollowedArtist);
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}

