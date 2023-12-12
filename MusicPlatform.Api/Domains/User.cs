namespace MusicPlatform.Api.Domains;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public ICollection<FavoriteArtist> Followings { get; set; } = new List<FavoriteArtist>();
    public ICollection<FavoriteTrack> LikedTracks { get; set; } = new List<FavoriteTrack>();
    public ICollection<FavoriteAlbum> LikedAlbums { get; set; } = new List<FavoriteAlbum>();
    public ICollection<FavoritePlaylist> LikedPlaylists { get; set; } = new List<FavoritePlaylist>();
    public ICollection<Playlist> UserPlaylists { get; set; } = new List<Playlist>();
    public ICollection<History> Histories { get; set; } = new List<History>();
}
