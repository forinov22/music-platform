using System;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record FavoriteAlbumDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AlbumId { get; set; }
        public DateOnly Date { get; set; }
        public UserDto User { get; set; }
        public AlbumDto Album { get; set; }
    }
}