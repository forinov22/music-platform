using System.Collections.Generic;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PlayCount { get; set; }
        public string CoverUrl { get; set; }
        public int ArtistId { get; set; }
        public ArtistDto Artist { get; set; }
        public ICollection<AlbumTrackDto> Tracks { get; set; }
        public ICollection<GenreDto> Genres { get; set; }
        public ICollection<FavoriteAlbumDto> Likes { get; set; }
    }
}