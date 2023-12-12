namespace MusicPlatform.Api.Domains;

public class Genre
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    public ICollection<Album> Albums { get; set; } = new List<Album>();
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
