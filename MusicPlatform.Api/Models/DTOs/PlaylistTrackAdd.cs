namespace MusicPlatform.Api.Models.DTOs;

public record PlaylistTrackAdd
{
    public int TrackId { get; set; }
    public int Order { get; set; }
}
