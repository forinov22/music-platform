using System.Collections.Generic;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record PlaylistDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PlayCount { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public ICollection<ArtistDto> Singers { get; set; }
        public ICollection<PlaylistTrackDto> Tracks { get; set; }
        public ICollection<GenreDto> Genres { get; set; }
        public ICollection<FavoritePlaylistDto> Likes { get; set; }
    }
}