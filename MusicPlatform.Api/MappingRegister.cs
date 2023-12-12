using Mapster;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api;

public class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForDestinationType<AlbumDto>().MaxDepth(2);
        config.ForDestinationType<ArtistDto>().MaxDepth(2);
        config.ForDestinationType<PlaylistDto>().MaxDepth(2);
        config.ForDestinationType<TrackDto>().MaxDepth(2);
        config.ForDestinationType<UserDto>().MaxDepth(2);
        config.ForDestinationType<PlaylistTrackDto>().MaxDepth(2);
        config.ForDestinationType<AlbumTrackDto>().MaxDepth(2);
        config.ForDestinationType<HistoryDto>().MaxDepth(2);
        config.ForDestinationType<GenreDto>().MaxDepth(2);
        config.ForDestinationType<FavoriteAlbumDto>().MaxDepth(2);
        config.ForDestinationType<FavoriteArtistDto>().MaxDepth(2);
        config.ForDestinationType<FavoritePlaylistDto>().MaxDepth(2);
        config.ForDestinationType<FavoritePlaylistDto>().MaxDepth(2);
    }
}