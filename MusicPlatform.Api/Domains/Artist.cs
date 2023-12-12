namespace MusicPlatform.Api.Domains;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int Streams { get; set; }
    public ICollection<FavoriteArtist> Followers { get; set; } = new List<FavoriteArtist>();
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Album> Albums { get; set; } = new List<Album>();
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
