using System.Collections.Generic;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record GenreDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<TrackDto> Tracks { get; set; }
        public ICollection<ArtistDto> Artists { get; set; }
        public ICollection<AlbumDto> Albums { get; set; }
        public ICollection<PlaylistDto> Playlists { get; set; }
    }
}