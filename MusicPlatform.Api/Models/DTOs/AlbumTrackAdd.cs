namespace MusicPlatform.Api.Models.DTOs;

public record AlbumTrackAdd
{
    public TrackAdd Track { get; set; } = null!;
    public int Order { get; set; }
}
