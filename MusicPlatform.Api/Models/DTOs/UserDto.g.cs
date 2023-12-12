using System.Collections.Generic;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Models.DTOs
{
    public partial record UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<FavoriteArtistDto> Followings { get; set; }
        public ICollection<FavoriteTrackDto> LikedTracks { get; set; }
        public ICollection<FavoriteAlbumDto> LikedAlbums { get; set; }
        public ICollection<FavoritePlaylistDto> LikedPlaylists { get; set; }
        public ICollection<PlaylistDto> UserPlaylists { get; set; }
        public ICollection<HistoryDto> Histories { get; set; }
    }
}