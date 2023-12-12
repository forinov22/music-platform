namespace MusicPlatform.Api.Domains;

public class Playlist
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int PlayCount { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Artist> Singers { get; set; } = new List<Artist>();
    public ICollection<PlaylistTrack> Tracks { get; set; } = new List<PlaylistTrack>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<FavoritePlaylist> Likes { get; set; } = new List<FavoritePlaylist>();
}
