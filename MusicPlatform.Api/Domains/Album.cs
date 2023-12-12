namespace MusicPlatform.Api.Domains;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int PlayCount { get; set; }
    public string CoverUrl { get; set; } = null!;
    public int ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;
    public ICollection<AlbumTrack> Tracks { get; set; } = new List<AlbumTrack>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<FavoriteAlbum> Likes { get; set; } = new List<FavoriteAlbum>();
}
