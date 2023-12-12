namespace MusicPlatform.Api.Domains;

public class AlbumTrack
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public int TrackId { get; set; }
    public int Order { get; set; }
    public Album Album { get; set; } = null!;
    public Track Track { get; set; } = null!;
}
