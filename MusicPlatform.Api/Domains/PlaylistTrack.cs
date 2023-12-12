namespace MusicPlatform.Api.Domains;

public class PlaylistTrack
{
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    public int TrackId { get; set; }
    public int Order { get; set; }
    public Playlist Playlist { get; set; } = null!;
    public Track Track { get; set; } = null!;
}
