using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Domains;

namespace MusicPlatform.Api.Data;

public class MusicPlatformContext : DbContext
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<AlbumTrack> AlbumTracks { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<FavoriteAlbum> FavoriteAlbums { get; set; }
    public DbSet<FavoriteArtist> FavoriteArtists { get; set; }
    public DbSet<FavoritePlaylist> FavoritePlaylists { get; set; }
    public DbSet<FavoriteTrack> FavoriteTracks { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<User> Users { get; set; }

    public MusicPlatformContext(DbContextOptions<MusicPlatformContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Track>().HasOne(e => e.AlbumTrack).WithOne(e => e.Track).HasForeignKey<Track>(e => e.AlbumTrackId);
    }
}
