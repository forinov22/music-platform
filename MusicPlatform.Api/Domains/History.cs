namespace MusicPlatform.Api.Domains;

public class History
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TrackId { get; set; }
    public int PlayCount { get; set; }
    public DateOnly DateOnly { get; set; }
    public User User { get; set; } = null!;
    public Track Track { get; set; } = null!;
}
