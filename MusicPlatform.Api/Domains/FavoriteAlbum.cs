namespace MusicPlatform.Api.Domains;

public class FavoriteAlbum
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AlbumId { get; set; }
    public DateOnly Date { get; set; }
    public User User { get; set; } = null!;
    public Album Album { get; set; } = null!;
}
