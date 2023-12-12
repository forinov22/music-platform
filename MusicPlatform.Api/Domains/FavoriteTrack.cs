namespace MusicPlatform.Api.Domains;

public class FavoriteTrack
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TrackId { get; set; }
    public DateOnly Date { get; set; }
    public User User { get; set; } = null!;
    public Track Track { get; set; } = null!;
}
