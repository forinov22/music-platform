using System;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record FavoritePlaylistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlaylistId { get; set; }
        public DateOnly Date { get; set; }
        public UserDto User { get; set; }
        public PlaylistDto Playlist { get; set; }
    }
}