namespace MusicPlatform.Api.Models.DTOs
{
    public partial record TrackDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AudioUrl { get; set; }
        public string CoverUrl { get; set; }
        public int PlayCount { get; set; }
        public int ArtistId { get; set; }
        public ArtistDto Artist { get; set; }
        public ICollection<FavoriteTrackDto> Likes { get; set; }
        public int? AlbumTrackId { get; set; }
        public AlbumTrackDto? AlbumTrack { get; set; }
        public ICollection<PlaylistTrackDto> PlaylistTracks { get; set; }
        public int GenreId { get; set; }
        public GenreDto Genre { get; set; }
        public ICollection<HistoryDto> Histories { get; set; }
    }
}