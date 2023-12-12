using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record PlaylistTrackDto
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
        public int Order { get; set; }
        public PlaylistDto Playlist { get; set; }
        public TrackDto Track { get; set; }
    }
}