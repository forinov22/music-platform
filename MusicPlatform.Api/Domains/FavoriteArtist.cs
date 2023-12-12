namespace MusicPlatform.Api.Domains;

public class FavoriteArtist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArtistId { get; set; }
    public DateOnly Date { get; set; }
    public User User { get; set; } = null!;
    public Artist Artist { get; set; } = null!;
}
