namespace MusicPlatform.Api.Models.DTOs;

public record AlbumAdd
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile CoverFile { get; set; } = null!;
    public ICollection<AlbumTrackAdd> Tracks { get; set; } = new List<AlbumTrackAdd>();
}
