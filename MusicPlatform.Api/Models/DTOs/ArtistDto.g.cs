namespace MusicPlatform.Api.Models.DTOs
{
    public partial record ArtistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public int Streams { get; set; }
        public ICollection<FavoriteArtistDto> Followers { get; set; }
        public ICollection<TrackDto> Tracks { get; set; }
        public ICollection<GenreDto> Genres { get; set; }
        public ICollection<AlbumDto> Albums { get; set; }
        public ICollection<PlaylistDto> Playlists { get; set; }
    }
}