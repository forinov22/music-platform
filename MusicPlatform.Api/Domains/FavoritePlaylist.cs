namespace MusicPlatform.Api.Domains;

public class FavoritePlaylist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PlaylistId { get; set; }
    public DateOnly Date { get; set; }
    public User User { get; set; } = null!;
    public Playlist Playlist { get; set; } = null!;
}
