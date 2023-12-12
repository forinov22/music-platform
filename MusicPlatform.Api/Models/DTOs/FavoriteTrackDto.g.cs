using System;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record FavoriteTrackDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TrackId { get; set; }
        public DateOnly Date { get; set; }
        public UserDto User { get; set; }
        public TrackDto Track { get; set; }
    }
}