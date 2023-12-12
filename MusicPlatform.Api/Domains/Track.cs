namespace MusicPlatform.Api.Domains;

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string AudioUrl { get; set; } = null!;
    public string CoverUrl { get; set; } = null!;
    public int PlayCount { get; set; }
    public int ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;
    public ICollection<FavoriteTrack> Likes { get; set; } = new List<FavoriteTrack>();
    public int? AlbumTrackId { get; set; }
    public AlbumTrack? AlbumTrack { get; set; }
    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
    public ICollection<History> Histories { get; set; } = new List<History>();
}
