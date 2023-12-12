using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record AlbumTrackDto
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int TrackId { get; set; }
        public int Order { get; set; }
        public AlbumDto Album { get; set; }
        public TrackDto Track { get; set; }
    }
}