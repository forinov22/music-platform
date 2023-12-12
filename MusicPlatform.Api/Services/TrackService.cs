using Mapster;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Domains;
using MusicPlatform.Api.Exceptions;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface ITrackService
{
    Task<IEnumerable<TrackDto>> GetAllTracksAsync();
    Task<TrackDto> GetTrackByIdAsync(int trackId);
    Task<TrackDto> CreateTrackAsync(int artistId, TrackAdd dto);
    Task<bool> DeleteTrackAsync(int trackId, int artistId);

    Task<IEnumerable<TrackDto>> GetUserLikedTracksAsync(int userId);
    Task<bool> AddLikedTrackToUserAsync(int userId, int trackId);
    Task<bool> DeleteUserLikedTrackAsync(int userId, int trackId);

    Task<HistoryDto> UpdateUserHistoryAsync(int userId, int trackId);
}

public class TrackService : ITrackService
{
    private readonly MusicPlatformContext context;
    private readonly IWebHostEnvironment environment;

    public TrackService(MusicPlatformContext context, IWebHostEnvironment environment)
    {
        this.context = context;
        this.environment = environment;
    }

    public async Task<IEnumerable<TrackDto>> GetAllTracksAsync()
    {
        return await context.Tracks.ProjectToType<TrackDto>()
            .ToListAsync();
    }

    public async Task<TrackDto> GetTrackByIdAsync(int trackId)
    {
        var track = await context.Tracks.ProjectToType<TrackDto>()
            .FirstOrDefaultAsync(t => t.Id == trackId);

        if (track == null)
            throw new NotFoundException(ExceptionMessages.TrackNotFound);

        return track;
    }

    public async Task<TrackDto> CreateTrackAsync(int artistId, TrackAdd dto)
    {
        var track = dto.Adapt<Track>();

        var artist = await context.Artists.FindAsync(artistId);
        var genre = await context.Genres.FindAsync(dto.GenreId);

        if (artist == null)
            throw new NotFoundException(ExceptionMessages.ArtistNotFound);

        if (genre == null)
            throw new NotFoundException(ExceptionMessages.GenreNotFound);

        var audioUrl = Path.Combine("tracks", Guid.NewGuid().ToString() + Path.GetExtension(dto.TrackFile.FileName));
        var audioUploadPath =  Path.Combine(environment.WebRootPath, audioUrl);

        await using (var fs = new FileStream(audioUploadPath, FileMode.Create)) {
            await dto.TrackFile.CopyToAsync(fs);
        }

        var coverUrl = Path.Combine("covers", "tracks", Guid.NewGuid().ToString() + Path.GetExtension(dto.CoverFile.FileName));
        var coverUploadPath = Path.Combine(environment.WebRootPath, coverUrl);

        await using (var fs = new FileStream(coverUploadPath, FileMode.Create)) {
            await dto.CoverFile.CopyToAsync(fs);
        }

        if (!artist.Genres.Contains(genre))
            artist.Genres.Add(genre);

        track.AudioUrl = audioUrl;
        track.CoverUrl = coverUrl;
        track.ArtistId = artistId;

        await context.Tracks.AddAsync(track);
        await context.SaveChangesAsync();
        return track.Adapt<TrackDto>();
    }

    public async Task<bool> DeleteTrackAsync(int trackId, int artistId)
    {
        var track = await context.Tracks.FindAsync(trackId);

        if (track == null)
            throw new NotFoundException(ExceptionMessages.TrackNotFound);

        context.Tracks.Remove(track);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TrackDto>> GetUserLikedTracksAsync(int userId)
    {
        return await context.FavoriteTracks.ProjectToType<FavoriteTrack>()
            .Where(ft => ft.UserId == userId)
            .Select(ft => ft.Track.Adapt<TrackDto>())
            .ToListAsync();
    }

    public async Task<bool> AddLikedTrackToUserAsync(int userId, int trackId)
    {
        var user = await context.Users.FindAsync(userId);
        var track = await context.Tracks.FindAsync(trackId);

        if (user == null)
            throw new NotFoundException(ExceptionMessages.UserNotFound);

        if (track == null)
            throw new NotFoundException(ExceptionMessages.TrackNotFound);

        var userLikedTrack = new FavoriteTrack { UserId = userId, TrackId = trackId, Date = DateOnly.FromDateTime(DateTime.UtcNow) };

        context.FavoriteTracks.Add(userLikedTrack);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserLikedTrackAsync(int userId, int trackId)
    {
        var userLikedTrack = await context.FavoriteTracks
            .FirstOrDefaultAsync(ul => ul.TrackId == trackId && ul.UserId == userId);

        if (userLikedTrack != null)
        {
            context.FavoriteTracks.Remove(userLikedTrack);
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<HistoryDto> UpdateUserHistoryAsync(int userId, int trackId)
    {
        var user = await context.Users.FindAsync(trackId);
        if (user == null)
            throw new NotFoundException(ExceptionMessages.UserNotFound);

        var track = await context.Tracks.FindAsync(trackId);
        if (track == null)
            throw new NotFoundException(ExceptionMessages.TrackNotFound);

        var existingHistory = await context.Histories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.TrackId == trackId);

        if (existingHistory != null) {
            existingHistory.PlayCount++;
            existingHistory.DateOnly = DateOnly.FromDateTime(DateTime.UtcNow);
            await context.SaveChangesAsync();
            return existingHistory.Adapt<HistoryDto>();
        }
        
        if (track.AlbumTrackId != null) {
            var albumTrack = await context.AlbumTracks.FindAsync(track.AlbumTrackId);
            Album? album = null;

            if (albumTrack != null)
                album = await context.Albums.FindAsync(albumTrack.AlbumId);

            if (album == null)
                throw new NotFoundException(ExceptionMessages.AlbumNotFound);

            album.PlayCount++;
        }

        var newHistory = new History { UserId = userId, TrackId = trackId, PlayCount = 1, DateOnly = DateOnly.FromDateTime(DateTime.UtcNow) };
        await context.Histories.AddAsync(newHistory);
        await context.SaveChangesAsync();
        return newHistory.Adapt<HistoryDto>();
    }
}
