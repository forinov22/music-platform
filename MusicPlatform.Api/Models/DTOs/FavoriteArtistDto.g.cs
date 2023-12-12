using System;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record FavoriteArtistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArtistId { get; set; }
        public DateOnly Date { get; set; }
        public UserDto User { get; set; }
        public ArtistDto Artist { get; set; }
    }
}