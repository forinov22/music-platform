namespace MusicPlatform.Api.Models.DTOs;

public record PlaylistAdd
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<PlaylistTrackAdd> Tracks { get; set; } = new List<PlaylistTrackAdd>();
}
